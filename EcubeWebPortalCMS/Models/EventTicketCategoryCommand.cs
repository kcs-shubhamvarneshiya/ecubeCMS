// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="EventTicketCategoryCommand.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
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
    /// Class EventTicketCategoryCommand.
    /// </summary>
    public partial class EventTicketCategoryCommand : IEventTicketCategoryCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private EventTicketCategoryDataContext objDataContext = null;

        /// <summary>
        /// Gets all CMS configuration for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item  &gt;.</returns>
        public List<SelectListItem> GetAllEventTicketCategoryForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objEventTicketCategoryList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new EventTicketCategoryDataContext())
                {
                    //// objEventTicketCategoryList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<MS_GetAllEventTicketCategoryResult> objEventTicketCategoryResultList = this.objDataContext.MS_GetAllEventTicketCategory().ToList();
                    if (objEventTicketCategoryResultList != null && objEventTicketCategoryResultList.Count > 0)
                    {
                        foreach (var item in objEventTicketCategoryResultList)
                        {
                            objEventTicketCategoryList.Add(new SelectListItem { Text = item.EventTicketCategoryName, Value = item.ID.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objEventTicketCategoryList;
        }

        /// <summary>
        /// Gets the CMS configuration by CMS configuration identifier.
        /// </summary>
        /// <param name="lgEventTicketCategoryId">The long CMS configuration identifier.</param>
        /// <returns>EventTicketCategory  Model.</returns>
        public EventTicketCategoryModel GetEventTicketCategoryByEventTicketCategoryId(int lgEventTicketCategoryId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            EventTicketCategoryModel objEventTicketCategoryModel = new EventTicketCategoryModel();
            try
            {
                using (this.objDataContext = new EventTicketCategoryDataContext())
                {
                    MS_GetByEventTicketCategoryIDResult item = this.objDataContext.MS_GetByEventTicketCategoryID(lgEventTicketCategoryId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objEventTicketCategoryModel.Id = item.ID;
                        objEventTicketCategoryModel.EventTicketCategoryName = item.EventTicketCategoryName;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objEventTicketCategoryModel;
        }

        /// <summary>
        /// Saves the CMS configuration.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveEventTicketCategory(EventTicketCategoryModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventTicketCategoryDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateEventTicketCategory((int)objSave.Id, objSave.EventTicketCategoryName, Convert.ToInt32(MySession.Current.UserId)).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.Id = result.ID;
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
        /// <param name="strEventTicketCategoryIdList">The string CMS configuration identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_Delete EventTicketCategory  Result.</returns>
        public MS_DeleteEventTicketCategoryResult DeleteEventTicketCategory(string strEventTicketCategoryIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            MS_DeleteEventTicketCategoryResult result = new MS_DeleteEventTicketCategoryResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventTicketCategoryDataContext())
                    {
                        result = this.objDataContext.MS_DeleteEventTicketCategory(strEventTicketCategoryIdList, lgDeletedBy).ToList().FirstOrDefault();
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
        /// Determines whether [is CMS configuration exists] [the specified long CMS configuration identifier].
        /// </summary>
        /// <param name="lgEventTicketCategoryId">The long CMS configuration identifier.</param>
        /// <param name="strEventTicketCategoryName">Name of the string CMS configuration.</param>
        /// <returns><c>true</c> if [is CMS configuration exists] [the specified long CMS configuration identifier]; otherwise, <c>false</c>.</returns>
        public bool IsEventTicketCategoryExists(long lgEventTicketCategoryId, string strEventTicketCategoryName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventTicketCategoryDataContext())
                {
                    if (this.objDataContext.EventTicketCategories.Where(x => x.Id != lgEventTicketCategoryId && x.EventTicketCategoryName == strEventTicketCategoryName && x.IsDeleted == false).Count() > 0)
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
        /// Searches the CMS configuration.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;MS_Search EventTicketCategory Result  &gt;.</returns>
        public List<MS_SearchEventTicketCategoryResult> SearchEventTicketCategory(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventTicketCategoryDataContext())
                {
                    List<MS_SearchEventTicketCategoryResult> objSearchEventTicketCategoryList = this.objDataContext.MS_SearchEventTicketCategory(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchEventTicketCategoryList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }
    }
}
