// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="IRoomBookingRequestCommand.cs" company="string.Empty">
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
    /// Interface IRoomBookingRequestCommand.
    /// </summary>
    public interface IRoomBookingRequestCommand
    {
        /// <summary>
        /// Gets all room booking request for drop down.
        /// </summary>
        /// <returns>List&lt;SelectList Item &gt;.</returns>
        List<SelectListItem> GetAllRoomBookingRequestForDropDown();

        /// <summary>
        /// Gets the room booking request by room booking request identifier.
        /// </summary>
        /// <param name="lgRoomBookingRequestId">The long room booking request identifier.</param>
        /// <returns>RoomBookingRequest  Model.</returns>
        RoomBookingRequestModel GetRoomBookingRequestByRoomBookingRequestId(long lgRoomBookingRequestId);

        /// <summary>
        /// Saves the room booking request.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveRoomBookingRequest(RoomBookingRequestModel objSave);

        /// <summary>
        /// Deletes the room booking request.
        /// </summary>
        /// <param name="strRoomBookingRequestList">The string room booking request list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteRoomBookingRequest Result.</returns>
        MS_DeleteRoomBookingRequestResult DeleteRoomBookingRequest(string strRoomBookingRequestList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is room booking request exists] [the specified long room booking request identifier].
        /// </summary>
        /// <param name="lgRoomBookingRequestId">The long room booking request identifier.</param>
        /// <param name="strName">Name of the string.</param>
        /// <returns><c>true</c> if [is room booking request exists] [the specified long room booking request identifier]; otherwise, <c>false</c>.</returns>
        bool IsRoomBookingRequestExists(long lgRoomBookingRequestId, string strName);

        /// <summary>
        /// Searches the room booking request.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <param name="sortedby">The sorted by.</param>
        /// <param name="roomType">Type of the room.</param>
        /// <returns>List&lt;MS_SearchRoomBookingRequest Result &gt;.</returns>
        List<MS_SearchRoomBookingRequestResult> SearchRoomBookingRequest(int inRow, int inPage, string strSearch, string strSort, int sortedby, long roomType);
    }
}
