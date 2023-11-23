// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="ISMSLogCommand.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface ISMSLogCommand.
    /// </summary>
    public interface ISMSLogCommand
    {
        /// <summary>
        /// Saves the SMS log.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveSMSLog(SMSLogModel objSave);

        /// <summary>
        /// Searches the SMS log.
        /// </summary>
        /// <param name="row">Parameter row.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="search">Parameter search.</param>
        /// <param name="sort">Parameter sort.</param>
        /// <param name="relaventId">The relevant identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="fromDate">From date parameter.</param>
        /// <param name="toDate">To date parameter.</param>
        /// <param name="status">The status parameter.</param>
        /// <returns>List&lt;SearchSMSLogResult    &gt;.</returns>
        List<SearchSMSLogResult> SearchSMSLog(int row, int page, string search, string sort, int relaventId, int moduleId, string fromDate, string toDate, string status);
    }
}
