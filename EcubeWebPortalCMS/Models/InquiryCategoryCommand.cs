

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

    public partial class InquiryCategoryCommand : IInquiryCategoryCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private InquiryCategoryDataContext objDataContext = null;

        /// <summary>
        /// Gets all CMS configuration for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item  &gt;.</returns>
        public List<SelectListItem> GetAllInquiryCategoryForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objInquiryCategoryList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new InquiryCategoryDataContext())
                {
                    objInquiryCategoryList.Add(new SelectListItem { Text = "Select Inquiry Category", Value = "0" });
                    List<MS_GetAllInquiryCategoryResult> objInquiryCategoryResultList = this.objDataContext.MS_GetAllInquiryCategory().ToList();
                    if (objInquiryCategoryResultList != null && objInquiryCategoryResultList.Count > 0)
                    {
                        foreach (var item in objInquiryCategoryResultList)
                        {
                            objInquiryCategoryList.Add(new SelectListItem { Text = item.CategoryName, Value = item.ID.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.InquiryCategory, MySession.Current.UserId);
            }

            return objInquiryCategoryList;
        }

        /// <summary>
        /// Gets the CMS configuration by CMS configuration identifier.
        /// </summary>
        /// <param name="lgInquiryCategoryId">The long CMS configuration identifier.</param>
        /// <returns>InquiryCategory  Model.</returns>
        public InquiryCategoryModel GetInquiryCategoryByInquiryCategoryId(int lgInquiryCategoryId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            InquiryCategoryModel objInquiryCategoryModel = new InquiryCategoryModel();
            try
            {
                using (this.objDataContext = new InquiryCategoryDataContext())
                {
                    MS_GetByInquiryCategoryIDResult item = this.objDataContext.MS_GetByInquiryCategoryID(lgInquiryCategoryId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objInquiryCategoryModel.Id = item.ID;
                        objInquiryCategoryModel.Category = item.Category;
                        objInquiryCategoryModel.MobileNumber = item.MobileNo;
                        objInquiryCategoryModel.Email = item.Email;
                        objInquiryCategoryModel.SeqNo = item.SeqNo;
                        objInquiryCategoryModel.CategoryImage = item.CategoryImage;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objInquiryCategoryModel;
        }

        /// <summary>
        /// Searches the CMS configuration.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;MS_Search InquiryCategory Result  &gt;.</returns>
        public List<MS_SearchInquiryCategoryResult> SearchInquiryCategory(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new InquiryCategoryDataContext())
                {
                    List<MS_SearchInquiryCategoryResult> objSearchInquiryCategoryList = this.objDataContext.MS_SearchInquiryCategory(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchInquiryCategoryList;
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
        /// Determines whether [is CMS configuration exists] [the specified long CMS configuration identifier].
        /// </summary>
        /// <param name="lgInquiryCategoryId">The long CMS configuration identifier.</param>
        /// <param name="strInquiryCategoryName">Name of the string CMS configuration.</param>
        /// <returns><c>true</c> if [is CMS configuration exists] [the specified long CMS configuration identifier]; otherwise, <c>false</c>.</returns>
        public bool IsInquiryCategoryExists(long lgInquiryCategoryId, string strInquiryCategoryName, int SeqNo)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new InquiryCategoryDataContext())
                {
                    if (this.objDataContext.InquiryCategories.Where(x => x.Id != lgInquiryCategoryId && x.Category == strInquiryCategoryName && x.IsDeleted == false).Count() > 0)
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

        public bool GetDuplicateSeq(long lgInquiryCategoryId, string strInquiryCategoryName, int SeqNo)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new InquiryCategoryDataContext())
                {
                    var seqNoDuplicate = this.objDataContext.InquiryCategories.Where(x => ((x.SeqNo == SeqNo && SeqNo != 0) || (x.SeqNo == 0 && SeqNo == 0)) && x.Id != lgInquiryCategoryId && !x.IsDeleted);

                    if (seqNoDuplicate.Count() > 0)
                    {
                        return true;
                    }

                    return false; // No duplicates found
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.MenuMobile, MySession.Current.UserId);
                return false;
            }
        }

        /// <summary>
        /// Saves the CMS configuration.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveInquiryCategory(InquiryCategoryModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new InquiryCategoryDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateInquiryCategory((int)objSave.Id, objSave.Category, objSave.CategoryImage, objSave.MobileNumber, objSave.Email, objSave.SeqNo, Convert.ToInt32(MySession.Current.UserId), PageMaster.InquiryCategory).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.Id = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return objSave.Id;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the CMS configuration.
        /// </summary>
        /// <param name="strInquiryCategoryIdList">The string CMS configuration identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteInquiryCategory  Result.</returns>
        public DeleteInquiryCategoryResult DeleteInquiryCategory(string strInquiryCategoryIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DeleteInquiryCategoryResult result = new DeleteInquiryCategoryResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new InquiryCategoryDataContext())
                    {
                        result = this.objDataContext.DeleteInquiryCategory(strInquiryCategoryIdList, (int)lgDeletedBy, PageMaster.InquiryCategory).ToList().FirstOrDefault();
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