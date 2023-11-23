// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : darpan
// Created          : 11-12-2016
//
// Last Modified By : darpan
// Last Modified On : 11-12-2016
// ***********************************************************************
// <copyright file="ServiceSupportLogCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>ServiceSupportLogCommand.cs</summary>
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
    using System.Web;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class ServiceSupportLogCommand.
    /// </summary>
    public partial class ServiceSupportLogCommand : IServiceSupportLogCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context variable.
        /// </summary>
        private ServiceSupportLogDataContext objDataContext = new ServiceSupportLogDataContext();

        /// <summary>
        /// Searches the service support log.
        /// </summary>
        /// <param name="inRow">The in row parameter.</param>
        /// <param name="inPage">The in page parameter.</param>
        /// <param name="strSearch">The string search parameter.</param>
        /// <param name="strddSearch">The STRDD search parameter.</param>
        /// <param name="strSort">The string sort parameter.</param>
        /// <returns>List&lt;CRM_SearchServiceSupportLogResult&gt; search service support log.</returns>
        public List<CRM_SearchServiceSupportLogResult> SearchServiceSupportLog(int inRow, int inPage, string strSearch, string strddSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new ServiceSupportLogDataContext())
                {
                    List<CRM_SearchServiceSupportLogResult> objSearch = this.objDataContext.CRM_SearchServiceSupportLog(inRow, inPage, strSearch, strddSearch, strSort).ToList();
                    return objSearch;
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
        /// Gets all status.
        /// </summary>
        /// <returns>List&lt;SelectListItem&gt; get all status.</returns>
        public List<SelectListItem> GetAllStatus()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objStatusList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new ServiceSupportLogDataContext())
                {
                    objStatusList.Add(new SelectListItem { Text = "--ALL--", Value = string.Empty });
                    List<CRMGetStatusAllResult> objStatusResult = this.objDataContext.CRMGetStatusAll().ToList();
                    if (objStatusResult.Count > 0)
                    {
                        foreach (var item in objStatusResult)
                        {
                            objStatusList.Add(new SelectListItem { Text = item.StatusDesc, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objStatusList;
        }
    }
}