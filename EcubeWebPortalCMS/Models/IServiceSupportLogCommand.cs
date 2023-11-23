// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : darpan
// Created          : 11-12-2016
//
// Last Modified By : darpan
// Last Modified On : 11-12-2016
// ***********************************************************************
// <copyright file="IServiceSupportLogCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>IServiceSupportLogCommand.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Interface IServiceSupportLog.
    /// </summary>
    public interface IServiceSupportLogCommand
    {
        /// <summary>
        /// Searches the service support log.
        /// </summary>
        /// <param name="inRow">The in row parameter.</param>
        /// <param name="inPage">The in page parameter.</param>
        /// <param name="strSearch">The string search parameter.</param>
        /// <param name="strSort">The string sort parameter.</param>
        /// <param name="strddSearch">The STRDD search parameter.</param>
        /// <returns>List&lt;CRM_SearchServiceSupportLogResult&gt; search service support log.</returns>
        List<CRM_SearchServiceSupportLogResult> SearchServiceSupportLog(int inRow, int inPage, string strSearch, string strSort, string strddSearch);

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>List&lt;SelectListItem&gt; get all status.</returns>
        List<SelectListItem> GetAllStatus();
    }
}