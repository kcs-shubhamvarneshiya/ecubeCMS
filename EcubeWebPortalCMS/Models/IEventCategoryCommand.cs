// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 12-08-2016
// ***********************************************************************
// <copyright file="IEventCategoryCommand.cs" company="string.Empty">
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
    /// Interface IEventCategoryCommand.
    /// </summary>
    public interface IEventCategoryCommand
    {
        /// <summary>
        /// Gets all event category for drop down.
        /// </summary>
        /// <returns>List SelectList Item.</returns>
        List<SelectListItem> GetAllEventCategoryForDropDown();

        /// <summary>
        /// Gets the event category by event category identifier.
        /// </summary>
        /// <param name="lgEventCategoryId">The LONG event category identifier.</param>
        /// <returns>Event Category Model.</returns>
        EventCategoryModel GetEventCategoryByEventCategoryId(int lgEventCategoryId);

        /// <summary>
        /// Saves the event category.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        long SaveEventCategory(EventCategoryModel objSave);

        /// <summary>
        /// Deletes the event category.
        /// </summary>
        /// <param name="strEventCategoryList">The string event category list.</param>
        /// <param name="lgDeletedBy">The LONG deleted by.</param>
        /// <returns>MS_Delete EventCategory  Result.</returns>
        MS_DeleteEventCategoryResult DeleteEventCategory(string strEventCategoryList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is event category exists] [the specified LONG event category identifier].
        /// </summary>
        /// <param name="lgEventCategoryId">The LONG event category identifier.</param>
        /// <param name="strEventCategoryName">Name of the string event category.</param>
        /// <returns><c>true</c> if [is event category exists] [the specified LONG event category identifier]; otherwise, <c>false</c>.</returns>
        bool IsEventCategoryExists(long lgEventCategoryId, string strEventCategoryName);

        /// <summary>
        /// Searches the event category.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List MS_SearchEvent CategoryResult.</returns>
        List<MS_SearchEventCategoryResult> SearchEventCategory(int inRow, int inPage, string strSearch, string strSort);
    }
}
