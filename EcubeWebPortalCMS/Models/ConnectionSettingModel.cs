// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : Maulik Shah
// Created          : 03-Feb-2017
//
// Last Modified By : Maulik Shah
// Last Modified On : 02-03-2017
// ***********************************************************************
// <copyright file="ConnectionSettingModel.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>ConnectionSettingModel.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    /// <summary>
    /// Class ConnectionSettingModel.
    /// </summary>
    public class ConnectionSettingModel
    {
        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the catalog.
        /// </summary>
        /// <value>The name of the catalog.</value>
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allow to change configuration.
        /// </summary>
        /// <value><c>true</c> if this instance is allow to change configuration; otherwise, <c>false</c>.</value>
        public bool IsAllowToChangeConfig { get; set; }
    }
}