// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="EventCategoryCommand.cs" company="string.Empty">
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
    /// Class EventCategoryCommand.
    /// </summary>
    public partial class EventCategoryCommand : IEventCategoryCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private EventCategoryDataContext objDataContext = null;

        /// <summary>
        /// Gets all CMS configuration for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item  &gt;.</returns>
        public List<SelectListItem> GetAllEventCategoryForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objEventCategoryList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new EventCategoryDataContext())
                {
                    objEventCategoryList.Add(new SelectListItem { Text = "Select Event Category", Value = "0" });
                    List<MS_GetAllEventCategoryResult> objEventCategoryResultList = this.objDataContext.MS_GetAllEventCategory().ToList();
                    if (objEventCategoryResultList != null && objEventCategoryResultList.Count > 0)
                    {
                        foreach (var item in objEventCategoryResultList)
                        {
                            objEventCategoryList.Add(new SelectListItem { Text = item.EventCategoryName, Value = item.ID.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.EventCategory, MySession.Current.UserId);
            }

            return objEventCategoryList;
        }

        /// <summary>
        /// Gets the CMS configuration by CMS configuration identifier.
        /// </summary>
        /// <param name="lgEventCategoryId">The long CMS configuration identifier.</param>
        /// <returns>EventCategory  Model.</returns>
        public EventCategoryModel GetEventCategoryByEventCategoryId(int lgEventCategoryId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            EventCategoryModel objEventCategoryModel = new EventCategoryModel();
            try
            {
                using (this.objDataContext = new EventCategoryDataContext())
                {
                    MS_GetByEventCategoryIDResult item = this.objDataContext.MS_GetByEventCategoryID(lgEventCategoryId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objEventCategoryModel.Id = item.ID;
                        objEventCategoryModel.EventCategoryName = item.EventCategoryName;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objEventCategoryModel;
        }

        /// <summary>
        /// Saves the CMS configuration.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveEventCategory(EventCategoryModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventCategoryDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateEventCategory((int)objSave.Id, objSave.EventCategoryName, Convert.ToInt32(MySession.Current.UserId));
                        if (result != null)
                        {
                            objSave.Id = result.Count();
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
        /// <param name="strEventCategoryIdList">The string CMS configuration identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_Delete EventCategory  Result.</returns>
        public MS_DeleteEventCategoryResult DeleteEventCategory(string strEventCategoryIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MS_DeleteEventCategoryResult result = new MS_DeleteEventCategoryResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventCategoryDataContext())
                    {
                        result = this.objDataContext.MS_DeleteEventCategory(strEventCategoryIdList, lgDeletedBy).ToList().FirstOrDefault();
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
        /// <param name="lgEventCategoryId">The long CMS configuration identifier.</param>
        /// <param name="strEventCategoryName">Name of the string CMS configuration.</param>
        /// <returns><c>true</c> if [is CMS configuration exists] [the specified long CMS configuration identifier]; otherwise, <c>false</c>.</returns>
        public bool IsEventCategoryExists(long lgEventCategoryId, string strEventCategoryName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new EventCategoryDataContext())
                {
                    if (this.objDataContext.EventCategories.Where(x => x.Id != lgEventCategoryId && x.EventCategoryName == strEventCategoryName && x.IsDeleted == false).Count() > 0)
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
        /// <returns>List&lt;MS_Search EventCategory Result  &gt;.</returns>
        public List<MS_SearchEventCategoryResult> SearchEventCategory(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new EventCategoryDataContext())
                {
                    List<MS_SearchEventCategoryResult> objSearchEventCategoryList = this.objDataContext.MS_SearchEventCategory(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchEventCategoryList;
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
