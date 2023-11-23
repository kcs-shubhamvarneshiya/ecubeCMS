// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-10-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-10-2016
// ***********************************************************************
// <copyright file="IServiceSupportCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>IServiceSupportCommand</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Interface IServiceSupportCommand.
    /// </summary>
    public interface IServiceSupportCommand
    {
        /// <summary>
        /// Searches the service support.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;CRMSearch Service Support Result&gt;.</returns>
        List<CRMSearchServiceSupportResult> SearchServiceSupport(int inRow, int inPage, string strSearch, string strSort);
        
        /// <summary>
        /// Gets the service support by identifier.
        /// </summary>
        /// <param name="lgServiceSupportById">The service support by identifier.</param>
        /// <returns>Service Support Model.</returns>
        ServiceSupportModel GetServiceSupportById(long lgServiceSupportById);

        /// <summary>
        /// Gets the status for drop down.
        /// </summary>
        /// <returns>List&lt;Select List Item&gt;.</returns>
        List<SelectListItem> GetStatusForDropDown();

        /// <summary>
        /// Gets the support type for drop down.
        /// </summary>
        /// <returns>List&lt;Select List Item&gt;.</returns>
        List<SelectListItem> GetSupportTypeForDropDown();

        /// <summary>
        /// Saves the service support.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Return long.</returns>
        long SaveServiceSupport(ServiceSupportModel objSave);

        /// <summary>
        /// Gets the service report by identifier.
        /// </summary>
        /// <param name="lgServiceSupportId">The service support identifier.</param>
        /// <returns>Service Support Model.</returns>
        ServiceSupportModel GetServiceReportById(long lgServiceSupportId);

        /// <summary>
        /// Gets the service support report by identifier.
        /// </summary>
        /// <param name="lgServiceSupportById">The service support by identifier.</param>
        /// <returns>Return Long.</returns>
        long GetServiceSupportReportById(long lgServiceSupportById);
    }
}
