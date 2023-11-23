// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="IEventTicketCategoryCommand.cs" company="string.Empty">
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
    /// Interface IEventTicketCategoryCommand.
    /// </summary>
    public interface IEventTicketCategoryCommand
    {
        /// <summary>
        /// Gets all CMS configuration for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item &gt;.</returns>
        List<SelectListItem> GetAllEventTicketCategoryForDropDown();

        /// <summary>
        /// Gets the CMS configuration by CMS configuration identifier.
        /// </summary>
        /// <param name="lgEventTicketCategoryId">The long CMS configuration identifier.</param>
        /// <returns>EventTicketCategory  Model.</returns>
        EventTicketCategoryModel GetEventTicketCategoryByEventTicketCategoryId(int lgEventTicketCategoryId);

        /// <summary>
        /// Saves the CMS configuration.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveEventTicketCategory(EventTicketCategoryModel objSave);

        /// <summary>
        /// Deletes the CMS configuration.
        /// </summary>
        /// <param name="strEventTicketCategoryList">The string CMS configuration list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_Delete EventTicketCategory  Result.</returns>
        MS_DeleteEventTicketCategoryResult DeleteEventTicketCategory(string strEventTicketCategoryList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is CMS configuration exists] [the specified long CMS configuration identifier].
        /// </summary>
        /// <param name="lgEventTicketCategoryId">The long CMS configuration identifier.</param>
        /// <param name="strEventTicketCategoryName">Name of the string CMS configuration.</param>
        /// <returns><c>true</c> if [is CMS configuration exists] [the specified long CMS configuration identifier]; otherwise, <c>false</c>.</returns>
        bool IsEventTicketCategoryExists(long lgEventTicketCategoryId, string strEventTicketCategoryName);

        /// <summary>
        /// Searches the CMS configuration.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;MS_Search EventTicketCategory  Result &gt;.</returns>
        List<MS_SearchEventTicketCategoryResult> SearchEventTicketCategory(int inRow, int inPage, string strSearch, string strSort);
    }
}
