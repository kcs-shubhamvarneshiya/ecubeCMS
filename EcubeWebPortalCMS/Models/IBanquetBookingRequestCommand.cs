// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="IBanquetBookingRequestCommand.cs" company="string.Empty">
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
    /// Interface IBanquetBookingRequestCommand.
    /// </summary>
    public interface IBanquetBookingRequestCommand
    {
        /// <summary>
        /// Gets all banquet booking request for drop down.
        /// </summary>
        /// <returns>List&lt; Select List Item &gt;.</returns>
        List<SelectListItem> GetAllBanquetBookingRequestForDropDown();

        /// <summary>
        /// Gets the banquet booking request by banquet booking request identifier.
        /// </summary>
        /// <param name="lgBanquetBookingRequestId">The long banquet booking request identifier.</param>
        /// <returns>Banquet Booking Request Model.</returns>
        BanquetBookingRequestModel GetBanquetBookingRequestByBanquetBookingRequestId(long lgBanquetBookingRequestId);

        /// <summary>
        /// Saves the banquet booking request.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveBanquetBookingRequest(BanquetBookingRequestModel objSave);

        /// <summary>
        /// Deletes the banquet booking request.
        /// </summary>
        /// <param name="strBanquetBookingRequestList">The string banquet booking request list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteBanquet Booking Request  Result.</returns>
        MS_DeleteBanquetBookingRequestResult DeleteBanquetBookingRequest(string strBanquetBookingRequestList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is banquet booking request exists] [the specified long banquet booking request identifier].
        /// </summary>
        /// <param name="lgBanquetBookingRequestId">The long banquet booking request identifier.</param>
        /// <param name="strName">Name of the string.</param>
        /// <returns><c>true</c> if [is banquet booking request exists] [the specified long banquet booking request identifier]; otherwise, <c>false</c>.</returns>
        bool IsBanquetBookingRequestExists(long lgBanquetBookingRequestId, string strName);

        /// <summary>
        /// Searches the banquet booking request.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <param name="sortedBy">The sorted by.</param>
        /// <param name="banquetType">Type of the banquet.</param>
        /// <returns>List&lt;MS_Search BanquetBookingRequest Result &gt;.</returns>
        List<MS_SearchBanquetBookingRequestResult> SearchBanquetBookingRequest(int inRow, int inPage, string strSearch, string strSort, int sortedBy, long banquetType);
    }
}
