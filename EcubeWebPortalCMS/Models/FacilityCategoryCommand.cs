// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-15-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="FacilityCategoryCommand.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>FacilityCategoryCommand.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class FacilityCategoryCommand.
    /// </summary>
    public class FacilityCategoryCommand : IFacilityCategory
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private FacilityDataContext objDataContext = new FacilityDataContext();

        /// <summary>
        /// Gets the category list.
        /// </summary>
        /// <returns>List&lt;FacilityCategoryModel&gt; get category list.</returns>
        public List<FacilityCategoryModel> GetCategoryList()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<FacilityCategoryModel> facilityCategoryList = new List<FacilityCategoryModel>();
            try
            {
                using (this.objDataContext = new FacilityDataContext())
                {
                    facilityCategoryList = this.objDataContext.FacilityCategories.Where(x => !x.IsDeleted).OrderByDescending(x => x.SequenceNo).Select(x => new FacilityCategoryModel
                    {
                        CategoryId = x.Id,
                        CategoryName = x.Name,
                        ImagePath = x.ImagePath
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

                throw;
            }

            return facilityCategoryList;
        }

        /// <summary>
        /// Searches the facility category.
        /// </summary>
        /// <param name="inRow">The in row Parameter.</param>
        /// <param name="inPage">The in page Parameter.</param>
        /// <param name="strSearch">The string search Parameter.</param>
        /// <param name="strSort">The string sort Parameter.</param>
        /// <returns>List&lt;SearchFacilityCategoryResult&gt; search facility category.</returns>
        public List<SearchFacilityCategoryResult> SearchFacilityCategory(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new FacilityDataContext())
                {
                    List<SearchFacilityCategoryResult> objSearchFacilityList = this.objDataContext.SearchFacilityCategory(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchFacilityList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Searches the facility group.
        /// </summary>
        /// <param name="inRow">The in row Parameter.</param>
        /// <param name="inPage">The in page Parameter.</param>
        /// <param name="strSearch">The string search Parameter.</param>
        /// <param name="strSort">The string sort Parameter.</param>
        /// <returns>List&lt;SearchFacilityGroupResult&gt; search facility group.</returns>
        public List<SearchFacilityGroupResult> SearchFacilityGroup(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new FacilityDataContext())
                {
                    List<SearchFacilityGroupResult> objSearchFacilityList = this.objDataContext.SearchFacilityGroup(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchFacilityList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Categories the list.
        /// </summary>
        /// <returns>List&lt;SelectListItem&gt; category list.</returns>
        public List<SelectListItem> GetAllCategoryList()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objCategoryList = new List<SelectListItem>();
            objCategoryList.Add(new SelectListItem { Text = "Select Category", Value = "0" });
            try
            {
                using (this.objDataContext = new FacilityDataContext())
                {
                    foreach (var item in this.objDataContext.FacilityCategories.Where(x => !x.IsDeleted))
                    {
                        objCategoryList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }

            return objCategoryList;
        }

        /// <summary>
        /// Gets the category by identifier.
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <returns>FacilityCategoryModel get category by identifier.</returns>
        public FacilityCategoryModel GetCategoryById(int id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            FacilityCategoryModel categoryById = new FacilityCategoryModel();
            try
            {
                using (this.objDataContext = new FacilityDataContext())
                {
                    categoryById = (from c in this.objDataContext.FacilityCategories
                                    where c.IsDeleted == false && c.Id == id
                                    select new FacilityCategoryModel
                                    {
                                        CategoryId = c.Id,
                                        CategoryName = c.Name,
                                        SequenceNo = c.SequenceNo,
                                        ImagePath = c.ImagePath
                                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }

            return categoryById;
        }

        /// <summary>
        /// Gets the facility by identifier.
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <returns>FacilityGroupModel get facility by identifier.</returns>
        public FacilityGroupModel GetFacilityById(int id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            FacilityGroupModel facilityById = new FacilityGroupModel();
            try
            {
                using (this.objDataContext = new FacilityDataContext())
                {
                    facilityById = (from f in this.objDataContext.Facilities
                                    join c in this.objDataContext.FacilityCategories on f.FacilityCategoryId equals c.Id
                                    where f.IsDeleted == false && f.Id == id && c.IsDeleted == false
                                    select new FacilityGroupModel
                                    {
                                        Id = f.Id,
                                        CategoryId = f.FacilityCategoryId,
                                        SequenceNo = f.SequenceNo,
                                        Name = f.Name,
                                        Description = f.Description,
                                        BannerImage = f.BannerImage,
                                        IconImage = f.IconImage
                                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }

            return facilityById;
        }

        /// <summary>
        /// Determines whether [is category exists] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns><c>true</c> if [is category exists] [the specified identifier]; otherwise, <c>false</c>.</returns>
        public bool IsCategoryExists(int id, string categoryName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new FacilityDataContext())
                {
                    if (this.objDataContext.FacilityCategories.Where(x => x.Id != id && x.Name == categoryName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Saves the category.
        /// </summary>
        /// <param name="objSave">The object save Parameter.</param>
        /// <returns>System save category.</returns>
        public int SaveCategory(FacilityCategoryModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int categoryId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new FacilityDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateFacilityCategory(objSave.CategoryId, objSave.CategoryName, objSave.ImagePath, objSave.SequenceNo, MySession.Current.UserId, PageMaster.FacilityCategory).FirstOrDefault();
                        if (result != null)
                        {
                            categoryId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return categoryId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Saves the facility.
        /// </summary>
        /// <param name="objSave">The object save Parameter.</param>
        /// <returns>System save facility.</returns>
        public int SaveFacility(FacilityGroupModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int facilityId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new FacilityDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateFacilityGroup(objSave.Id, objSave.CategoryId, objSave.SequenceNo, objSave.Name, objSave.Description, objSave.BannerImage, objSave.IconImage, MySession.Current.UserId, PageMaster.FacilityCategory).FirstOrDefault();
                        if (result != null)
                        {
                            facilityId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return facilityId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the category.
        /// </summary>
        /// <param name="strCategoryId">The string category identifier Parameter.</param>
        /// <param name="userId">The user identifier Parameter.</param>
        /// <returns>DeleteFacilityCategoryResult delete category.</returns>
        public DeleteFacilityCategoryResult DeleteCategory(string strCategoryId, int userId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DeleteFacilityCategoryResult result = new DeleteFacilityCategoryResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new FacilityDataContext())
                    {
                        result = this.objDataContext.DeleteFacilityCategory(strCategoryId, userId, PageMaster.FacilityCategory).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return result;
        }

        /// <summary>
        /// Deletes the facility.
        /// </summary>
        /// <param name="strFacilityId">The string facility identifier Parameter.</param>
        /// <param name="userId">The user identifier Parameter.</param>
        /// <returns>DeleteFacilityResult delete facility.</returns>
        public DeleteFacilityResult DeleteFacility(string strFacilityId, int userId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DeleteFacilityResult result = new DeleteFacilityResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new FacilityDataContext())
                    {
                        result = this.objDataContext.DeleteFacility(strFacilityId, userId, PageMaster.FacilityCategory).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return result;
        }
    }
}