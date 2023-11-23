// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-21-2016
// ***********************************************************************
// <copyright file="IRoomBookingCommand.cs" company="string.Empty">
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
    /// Interface IRoomBookingCommand.
    /// </summary>
    public interface IRoomBookingCommand
    {
        /// <summary>
        /// Gets all room booking for drop down.
        /// </summary>
        /// <returns>List&lt;SelectList Item &gt;.</returns>
        List<SelectListItem> GetAllRoomBookingForDropDown();

        /// <summary>
        /// Gets the room booking by room booking identifier.
        /// </summary>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        /// <returns>RoomBooking  Model.</returns>
        RoomBookingModel GetRoomBookingByRoomBookingId(long lgRoomBookingId);

        /// <summary>
        /// Saves the room booking.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <param name="objRoomBookingDetail">The object room booking detail.</param>
        /// <param name="objRoomBookingGallary">The object room booking gallery.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveRoomBooking(RoomBookingModel objSave, List<RoomBookingModel> objRoomBookingDetail, List<RoomBookingModel> objRoomBookingGallary);

        /// <summary>
        /// Deletes the room booking.
        /// </summary>
        /// <param name="strRoomBookingList">The string room booking list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteRoom Booking  Result.</returns>
        MS_DeleteRoomBookingResult DeleteRoomBooking(string strRoomBookingList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is room booking exists] [the specified long room booking identifier].
        /// </summary>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        /// <param name="strRoomBookingName">Name of the string room booking.</param>
        /// <returns><c>true</c> if [is room booking exists] [the specified long room booking identifier]; otherwise, <c>false</c>.</returns>
        bool IsRoomBookingExists(long lgRoomBookingId, string strRoomBookingName);

        /// <summary>
        /// Searches the room booking.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;MS_SearchRoomBooking Result &gt;.</returns>
        List<MS_SearchRoomBookingResult> SearchRoomBooking(int inRow, int inPage, string strSearch, string strSort);
        
        /// <summary>
        /// Saves the room booking detail.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveRoomBookingDetail(RoomBookingModel objSave);

        /// <summary>
        /// Deletes the room booking detail.
        /// </summary>
        /// <param name="strRoomBookingDetailList">The string room booking detail list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteRoomBookingTextDetail Result.</returns>
        MS_DeleteRoomBookingTextDetailResult DeleteRoomBookingDetail(string strRoomBookingDetailList, long lgDeletedBy);

        /// <summary>
        /// Gets the room booking detail by room booking identifier.
        /// </summary>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        /// <returns>List&lt;RoomBookingModel &gt;.</returns>
        List<RoomBookingModel> GetRoomBookingDetailByRoomBookingId(long lgRoomBookingId);

        /// <summary>
        /// Saves the room booking gallery.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveRoomBookingGallary(RoomBookingModel objSave);

        /// <summary>
        /// Deletes the room booking gallery.
        /// </summary>
        /// <param name="strRoomBookingGallaryList">The string room booking gallery list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteRoomBooking Gallery  Result.</returns>
        MS_DeleteRoomBookingGallaryResult DeleteRoomBookingGallary(string strRoomBookingGallaryList, long lgDeletedBy);

        /// <summary>
        /// Gets the room booking gallery by room booking identifier.
        /// </summary>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        /// <returns>List;RoomBooking Model &gt;.</returns>
        List<RoomBookingModel> GetRoomBookingGallaryByRoomBookingId(long lgRoomBookingId);
    }
}
