using EcubeWebPortalCMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Models
{
    public class MenuMasterCommand : IMenuMaster
    {
        /// <summary>
        /// The object data context.
        /// </summary>
        private MenuMobileDataContext menuMobileDataContext = new MenuMobileDataContext();

        public int MenuMobile(MenuMobileModel menuMobileModel)
        {
            try
            {
                int MenuId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.menuMobileDataContext = new MenuMobileDataContext())
                    {
                        var result = this.menuMobileDataContext.InsertOrUpdateMenu(menuMobileModel.MenuId, menuMobileModel.ParentMenuId, menuMobileModel.SeqNo,
                            menuMobileModel.MenuKey, menuMobileModel.MenuDisplayName, menuMobileModel.MenuIcon, MySession.Current.UserId, PageMaster.MenuMobile).FirstOrDefault();
                        if (result != null)
                        {
                            MenuId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return MenuId;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FacilityCategory, MySession.Current.UserId);
                return 0;
            }
        }

        public List<SubMenuMobileModel> SubMenuMobile(List<SubMenuMobileModel> subMenuMobileModelList, int ParentMenuId)
        {
            try
            {
                List<SubMenuMobileModel> subMenuMobileModels = new List<SubMenuMobileModel>();
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.menuMobileDataContext = new MenuMobileDataContext())
                    {

                        foreach (var subMenuMobileModel in subMenuMobileModelList)
                        {
                            if (subMenuMobileModel.SubisDeleted == false)
                            {
                                var result = this.menuMobileDataContext.InsertOrUpdateSubMenu(subMenuMobileModel.SubMenuId, ParentMenuId, subMenuMobileModel.SubSeqNo,
                                                   subMenuMobileModel.SubMenuKey, subMenuMobileModel.SubMenuDisplayName, subMenuMobileModel.SubIconImages,
                                                   MySession.Current.UserId, PageMaster.MenuMobile);

                                if (result != null)
                                {
                                    subMenuMobileModels.Add(new SubMenuMobileModel
                                    {
                                        SubMenuId = result.FirstOrDefault().InsertedSubMenuId,
                                        SubIconImages = subMenuMobileModel.SubIconImages
                                    });
                                }
                            }
                            else
                            {
                                if (subMenuMobileModel.SubMenuId != 0 && subMenuMobileModel.SubMenuId > 0)
                                {
                                    var removeRow = this.menuMobileDataContext.DeleteMobileMenu(subMenuMobileModel.SubMenuId.ToString(), MySession.Current.UserId, PageMaster.MenuMobile);
                                }
                            }
                        }


                    }

                    scope.Complete();
                }

                return subMenuMobileModels;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FacilityCategory, MySession.Current.UserId);
                return new List<SubMenuMobileModel>();
            }
        }

        public List<SearchMenuMobileResult> SearchMenuMobile(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.menuMobileDataContext = new MenuMobileDataContext())
                {
                    List<SearchMenuMobileResult> objSearchMenuList = this.menuMobileDataContext.SearchMenuMobile(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchMenuList;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FacilityCategory, MySession.Current.UserId);
                return null;
            }
        }

        public MenuMobileModel GetMenuDetailsByMenuId(int MenuId)
        {
            MenuMobileModel menuMobileModel = new MenuMobileModel();
            try
            {
                using (this.menuMobileDataContext = new MenuMobileDataContext())
                {
                    menuMobileModel = (from m in this.menuMobileDataContext.MenuMobiles
                                       where m.IsDeleted == false && m.MenuId == MenuId
                                       select new MenuMobileModel
                                       {
                                           MenuId = m.MenuId,
                                           ParentMenuId = m.ParentMenuId,
                                           SeqNo = m.SeqNo,
                                           MenuKey = m.MenuKey,
                                           MenuDisplayName = m.MenuDisplayName,
                                           MenuMobileModelList = GetSubMenuList(MenuId),
                                       }).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return menuMobileModel;
        }

        private List<SubMenuMobileModel> GetSubMenuList(int menuId)
        {
            List<SubMenuMobileModel> subMenuList = new List<SubMenuMobileModel>();
            try
            {
                using (this.menuMobileDataContext = new MenuMobileDataContext())
                {
                    subMenuList = (from m in this.menuMobileDataContext.MenuMobiles
                                   where m.IsDeleted == false && m.ParentMenuId == menuId
                                   select new SubMenuMobileModel
                                   {
                                       SubMenuId = m.MenuId,
                                       SubParentMenuId = m.ParentMenuId,
                                       SubSeqNo = m.SeqNo,
                                       SubMenuKey = m.MenuKey,
                                       SubMenuDisplayName = m.MenuDisplayName,
                                       SubIconImages = m.MenuIcon,
                                   }).ToList(); // Use ToList() to materialize the query result into a List
                }
            }
            catch (Exception)
            {
                throw;
            }

            return subMenuList;
        }

        public DeleteMobileMenuResult DeleteMobileMenu(string strMenuId, int userId)
        {
            DeleteMobileMenuResult result = new DeleteMobileMenuResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.menuMobileDataContext = new MenuMobileDataContext())
                    {
                        result = this.menuMobileDataContext.DeleteMobileMenu(strMenuId, userId, PageMaster.MenuMobile).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.MenuMobile, MySession.Current.UserId);
            }

            return result;
        }

        public bool IsMenuExists(int MenuId, string MenuKey, string MenuDisplayName, int? SeqNo)
        {
            try
            {
                using (this.menuMobileDataContext = new MenuMobileDataContext())
                {
                    // Check if there are any records with the same MenuKey, MenuDisplayName, and SeqNo but different MenuId.                    
                    bool exists = this.menuMobileDataContext.MenuMobiles
                        .Any(x => x.MenuId != MenuId && ((x.MenuKey == MenuKey) || (x.MenuDisplayName == MenuDisplayName) || ((x.SeqNo == SeqNo && SeqNo != null) || (x.SeqNo == null && SeqNo == null))) && x.IsDeleted == false && x.ParentMenuId == 0);

                    return exists;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.MenuMobile, MySession.Current.UserId);
                return false;

            }
        }

        public string GetDuplicateField(int MenuId, string MenuKey, string MenuDisplayName, int? SeqNo)
        {
            try
            {
                using (this.menuMobileDataContext = new MenuMobileDataContext())
                {
                    bool menuKeyDuplicate = this.menuMobileDataContext.MenuMobiles.Any(x => x.MenuKey == MenuKey && x.MenuId != MenuId && x.IsDeleted == false && x.ParentMenuId == 0);
                    bool menuDisplayNameDuplicate = this.menuMobileDataContext.MenuMobiles.Any(x => x.MenuDisplayName == MenuDisplayName && x.MenuId != MenuId && x.IsDeleted == false && x.ParentMenuId == 0);
                    bool seqNoDuplicate = this.menuMobileDataContext.MenuMobiles.Any(x => ((x.SeqNo == SeqNo && SeqNo != null) || (x.SeqNo == null && SeqNo == null)) && x.MenuId != MenuId && x.IsDeleted == false && x.ParentMenuId == 0);

                    if (menuKeyDuplicate && menuDisplayNameDuplicate && seqNoDuplicate)
                    {
                        return Functions.DuplicateFields.AllFields; // All fields have duplicates
                    }
                    else if (menuKeyDuplicate)
                    {
                        return Functions.DuplicateFields.MenuKey;
                    }
                    else if (menuDisplayNameDuplicate)
                    {
                        return Functions.DuplicateFields.MenuDisplayName;
                    }
                    else if (seqNoDuplicate)
                    {
                        return Functions.DuplicateFields.SeqNo;
                    }

                    return Functions.DuplicateFields.None; // No duplicates found
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.MenuMobile, MySession.Current.UserId);
                return Functions.DuplicateFields.None;
            }
        }
    }
}
