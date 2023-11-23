// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="IEventBookigCommand.cs" company="string.Empty">
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
    /// Interface ICMSConfigCommand.
    /// </summary>
    public interface IEventBookigCommand
    {
        /// <summary>
        /// Gets the event by event identifier.
        /// </summary>
        /// <param name="lgEventId">The LONG event category identifier.</param>
        /// <returns>Event Booking Model.</returns>
        EventBookingModel GetEventByEventId(int lgEventId);

        /// <summary>
        /// Saves the event.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        long SaveEvent(EventBookingModel objSave);

        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="strEventList">The string event list.</param>
        /// <param name="lgDeletedBy">The LONG deleted by.</param>
        /// <returns>MS_Delete Event Result.</returns>
        MS_DeleteEventResult DeleteEvent(string strEventList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is event exists] [the specified LONG event identifier].
        /// </summary>
        /// <param name="lgEventId">The LONG event identifier.</param>
        /// <param name="strEventName">Name of the string event.</param>
        /// <returns><c>true</c> if [is event exists] [the specified LONG event identifier]; otherwise, <c>false</c>.</returns>
        bool IsEventExists(long lgEventId, string strEventName);

        /// <summary>
        /// Gets the event schedule identifier.
        /// </summary>
        /// <param name="lgEventScheduleId">The LONG event identifier.</param>
        /// <returns>Event Schedule Model.</returns>
        EventScheduleModel GetEventScheduleId(int lgEventScheduleId);

        /// <summary>
        /// Saves the event.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        long SaveEventSchedule(EventScheduleModel objSave);

        /// <summary>
        /// Deletes the event schedule.
        /// </summary>
        /// <param name="strEventScheduleList">The string event schedule list.</param>
        /// <param name="lgDeletedBy">The LONG deleted by.</param>
        /// <returns>MS_DeleteEvent schedule Result.</returns>
        MS_DeleteEventscheduleResult DeleteEventSchedule(string strEventScheduleList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is event schedule exists] [the specified LONG event schedule identifier].
        /// </summary>
        /// <param name="lgEventScheduleId">The LONG event schedule identifier.</param>
        /// <param name="eventFromDate">Name of the LONG eventDate schedule.</param>
        /// <param name="eventToDate">Name of the LONG eventDate schedule.</param>
        /// <returns><c>true</c> if [is event schedule exists] [the specified LONG event schedule identifier]; otherwise, <c>false</c>.</returns>
        bool IsEventScheduleExists(long lgEventScheduleId, DateTime eventFromDate, DateTime eventToDate);

        /// <summary>
        /// Gets the event rate identifier.
        /// </summary>
        /// <param name="lgEventRateId">The LONG event rate identifier.</param>
        /// <returns>Event Rate Model.</returns>
        EventRateModel GetEventRateId(int lgEventRateId);

        /// <summary>
        /// Saves the event.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        long SaveEventRate(EventRateModel objSave);

        /// <summary>
        /// Deletes the event rate.
        /// </summary>
        /// <param name="strEventRateList">The string event rate list.</param>
        /// <param name="lgDeletedBy">The LONG deleted by.</param>
        /// <returns>MS_DeleteEventRate Result.</returns>
        MS_DeleteEventRateResult DeleteEventRate(string strEventRateList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is event rate exists] [the specified LONG event rate identifier].
        /// </summary>
        /// <param name="eventId">Name of the LONG event rate.</param>
        /// <param name="eventTicketCategoryId">The LONG event rate identifier.</param>
        /// <returns><c>true</c> if [is event rate exists] [the specified LONG event rate identifier]; otherwise, <c>false</c>.</returns>
        bool IsEventRateExists(long eventId, int eventTicketCategoryId);

        /// <summary>
        /// Searches the event.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <param name="completedEvent">The boolean complete Event.</param>
        /// <returns>List MS_SearchEventResult.</returns>
        List<MS_SerachEventResult> SearchEvent(int inRow, int inPage, string strSearch, string strSort, int completedEvent);

        /// <summary>
        /// Searches the event schedule.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List MS_SearchEventSchedule Result.</returns>
        List<MS_SearchEventScheduleResult> SearchEventSchedule(int eventId, int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Searches the event rate.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List MS_SearchEventRate Result.</returns>
        List<MS_SearchEventRateResult> SearchEventRate(int eventId, int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Accounts the ledger list.
        /// </summary>
        /// <param name="accountHead">The account head.</param>
        /// <param name="shortName">The short name.</param>
        /// <returns>List CRM_GetAccountLedgerList_Result.</returns>
        List<CRM_GetAccountLedgerList_Result> AccountLedgerList(string accountHead = "", string shortName = "");

        /// <summary>
        /// Gets all tax master for drop down.
        /// </summary>
        /// <returns>List&lt;SelectListItem&gt; get all tax master for drop down.</returns>
        List<SelectListItem> GetAllTaxMasterForDropDown();

        /// <summary>
        /// Gets all account head for drop down.
        /// </summary>
        /// <returns>List&lt;SelectListItem&gt; get all account head for drop down.</returns>
        List<SelectListItem> GetAllAccountHeadForDropDown();

        

        /// <summary>
        /// Gets all event for report.
        /// </summary>
        /// <returns>List of get all today's event for report.</returns>
        List<SelectListItem> GetAllEventForReport(DateTime eventDate);

        /// <summary>
        /// Events the booking report.
        /// </summary>
        /// <param name="eventId">The event identifier parameter.</param>
        /// <param name="ticketId">The ticket identifier parameter.</param>
        /// <param name="membercode">The member code parameter.</param>
        /// <param name="eventDate">The event Date parameter.</param>
        /// <returns>List of event booking report.</returns>
        List<MS_EventBookingReportResult> EventBookingReport(int eventId, int ticketId, string membercode, DateTime eventDate, int rows, int page);
    }
}