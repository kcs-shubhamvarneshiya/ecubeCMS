// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-12-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-12-2016
// ***********************************************************************
// <copyright file="ICustomPageCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>ICustomPageCommand.</summary>
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
    /// Interface ICustomPageCommand.
    /// </summary>
    public interface ICustomPageCommand
    {
        /// <summary>
        /// Gets the vendor by identifier.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <param name="pageName">Name of the page.</param>
        /// <returns>Vendor Model.</returns>
        CustomPageModel GetCustomPageById(int pageId, string pageName);

        /// <summary>
        /// Saves the vendor.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System long.</returns>
        long SaveCustomPage(CustomPageModel objSave);
    }
}
