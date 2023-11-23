// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="IEmailLogCommand.cs" company="string.Empty">
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
    /// Interface IEmailLogCommand.
    /// </summary>
    public interface IEmailLogCommand
    {
        /// <summary>
        /// Saves the email log.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveEmailLog(EmailLogModel objSave);

        /// <summary>
        /// Searches the email log.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <param name="relaventId">The relevant identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="fromDate">From date parameter.</param>
        /// <param name="toDate">To date parameter.</param>
        /// <param name="status">The status parameter.</param>
        /// <returns>List&lt;SearchEmail Log Result &gt;.</returns>
        List<SearchEmailLogResult> SearchEmailLog(int inRow, int inPage, string strSearch, string strSort, int relaventId, int moduleId, string fromDate, string toDate, string status);
    }
}
