// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : darpan
// Created          : 11-10-2016
//
// Last Modified By : darpan
// Last Modified On : 11-10-2016
// ***********************************************************************
// <copyright file="IServiceSupportTypeCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>IServiceSupportTypeCommand.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Interface IServiceSupportTypeCommand.
    /// </summary>
    public interface IServiceSupportTypeCommand
    {
        /// <summary>
        /// Searches the type of the service support.
        /// </summary>
        /// <param name="inRow">The in row parameter.</param>
        /// <param name="inPage">The in page parameter.</param>
        /// <param name="strSearch">The string search parameter.</param>
        /// <param name="strSort">The string sort parameter.</param>
        /// <returns>List&lt;CRMSearchServiceSupportTypeResult&gt; search service support type.</returns>
        List<CRMSearchServiceSupportTypeResult> SearchServiceSupportType(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Saves the type of the service support.
        /// </summary>
        /// <param name="objSave">The object save parameter.</param>
        /// <returns>System.Integer save service support type.</returns>
        long SaveServiceSupportType(ServiceSupportTypeModel objSave);

        /// <summary>
        /// Deletes the type of the service support.
        /// </summary>
        /// <param name="strSupportIdList">The string support identifier list parameter.</param>
        /// <param name="lgDeletedBy">The long deleted by parameter.</param>
        /// <returns>CRM_DeleteServiceSupportTypeResult delete service support type.</returns>
        CRM_DeleteServiceSupportTypeResult DeleteServiceSupportType(string strSupportIdList, int lgDeletedBy);

        /// <summary>
        /// Gets the service support type by identifier.
        /// </summary>
        /// <param name="id">The identifier parameter.</param>
        /// <returns>ServiceSupportTypeModel get service support type by identifier.</returns>
        ServiceSupportTypeModel GetServiceSupportTypeById(int id);

        /// <summary>
        /// Determines whether [is type name exists] [the specified long name identifier].
        /// </summary>
        /// <param name="lgNameId">The long name identifier parameter.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns><c>true</c> if [is type name exists] [the specified long name identifier]; otherwise, <c>false</c>.</returns>
        bool IsTypeNameExists(long lgNameId, string typeName);
    }
}