// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="IBanquetCommand.cs" company="string.Empty">
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
    using System.Web.Mvc;

    /// <summary>
    /// Interface IBanquetCommand.
    /// </summary>
    public interface IBanquetCommand
    {
        /// <summary>
        /// Gets all banquet for drop down.
        /// </summary>
        /// <returns>List&lt; SelectListItem  &gt;.</returns>
        List<SelectListItem> GetAllBanquetForDropDown();

        /// <summary>
        /// Gets the banquet by banquet identifier.
        /// </summary>
        /// <param name="lgBanquetId">The long banquet identifier.</param>
        /// <returns>Banquet  Model.</returns>
        BanquetModel GetBanquetByBanquetId(long lgBanquetId);

        /// <summary>
        /// Saves the banquet.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <param name="objBanquetDetail">The object banquet detail.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveBanquet(BanquetModel objSave, List<BanquetModel> objBanquetDetail);

        /// <summary>
        /// Deletes the banquet.
        /// </summary>
        /// <param name="strBanquetList">The string banquet list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_Delete Banquet  Result.</returns>
        MS_DeleteBanquetResult DeleteBanquet(string strBanquetList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is banquet exists] [the specified long banquet identifier].
        /// </summary>
        /// <param name="lgBanquetId">The long banquet identifier.</param>
        /// <param name="strBanquetName">Name of the string banquet.</param>
        /// <returns><c>true</c> if [is banquet exists] [the specified long banquet identifier]; otherwise, <c>false</c>.</returns>
        bool IsBanquetExists(long lgBanquetId, string strBanquetName);

        /// <summary>
        /// Searches the banquet.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;MS_Search BanquetResult  &gt;.</returns>
        List<MS_SearchBanquetResult> SearchBanquet(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Saves the banquet detail.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveBanquetDetail(BanquetModel objSave);

        /// <summary>
        /// Deletes the banquet detail.
        /// </summary>
        /// <param name="strBanquetDetailList">The string banquet detail list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_Delete Banquet Detail  Result.</returns>
        MS_DeleteBanquetDetailResult DeleteBanquetDetail(string strBanquetDetailList, long lgDeletedBy);

        /// <summary>
        /// Gets the banquet detail by banquet identifier.
        /// </summary>
        /// <param name="lgBanquetId">The long banquet identifier.</param>
        /// <returns>List;Banquet Model;.</returns>
        List<BanquetModel> GetBanquetDetailByBanquetId(long lgBanquetId);
    }
}
