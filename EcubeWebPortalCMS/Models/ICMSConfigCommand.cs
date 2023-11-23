// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="ICMSConfigCommand.cs" company="string.Empty">
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
    public interface ICMSConfigCommand
    {
        /// <summary>
        /// Gets all CMS configuration for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item &gt;.</returns>
        List<SelectListItem> GetAllCMSConfigForDropDown();

        /// <summary>
        /// Gets the CMS configuration by CMS configuration identifier.
        /// </summary>
        /// <param name="lgCMSConfigId">The long CMS configuration identifier.</param>
        /// <returns>CMSConfig  Model.</returns>
        CMSConfigModel GetCMSConfigByCMSConfigId(long lgCMSConfigId);

        /// <summary>
        /// Saves the CMS configuration.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveCMSConfig(CMSConfigModel objSave);

        /// <summary>
        /// Deletes the CMS configuration.
        /// </summary>
        /// <param name="strCMSConfigList">The string CMS configuration list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_Delete CMSConfig  Result.</returns>
        MS_DeleteCMSConfigResult DeleteCMSConfig(string strCMSConfigList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is CMS configuration exists] [the specified long CMS configuration identifier].
        /// </summary>
        /// <param name="lgCMSConfigId">The long CMS configuration identifier.</param>
        /// <param name="strCMSConfigName">Name of the string CMS configuration.</param>
        /// <returns><c>true</c> if [is CMS configuration exists] [the specified long CMS configuration identifier]; otherwise, <c>false</c>.</returns>
        bool IsCMSConfigExists(long lgCMSConfigId, string strCMSConfigName);

        /// <summary>
        /// Searches the CMS configuration.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;MS_Search CMSConfig  Result &gt;.</returns>
        List<MS_SearchCMSConfigResult> SearchCMSConfig(int inRow, int inPage, string strSearch, string strSort);
    }
}
