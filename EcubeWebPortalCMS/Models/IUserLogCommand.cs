// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="IUserLogCommand.cs" company="string.Empty">
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
    /// Interface IUserLogCommand.
    /// </summary>
    public interface IUserLogCommand
    {
        /// <summary>
        /// Saves the login history.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveLoginHistory(UserLogModel objSave);

        /// <summary>
        /// Searches the user log.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;SearchUserLog Result &gt;.</returns>
        List<SearchUserLogResult> SearchUserLog(int inRow, int inPage, string strSearch, string strSort);
    }
}
