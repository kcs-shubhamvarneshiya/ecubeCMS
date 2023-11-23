// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="IErrorLogCommand.cs" company="string.Empty">
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
    /// Interface IErrorLogCommand.
    /// </summary>
    public interface IErrorLogCommand
    {
        /// <summary>
        /// Searches the error log.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="sstrearch">The stretch.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List;Search ErrorLog Result;.</returns>
        List<SearchErrorLogResult> SearchErrorLog(int inRow, int inPage, string sstrearch, string strSort);
    }
}
