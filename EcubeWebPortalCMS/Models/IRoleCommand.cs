// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="IRoleCommand.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Interface IRoleCommand.
    /// </summary>
    public interface IRoleCommand
    {
        /// <summary>
        /// Gets all role for drop down.
        /// </summary>
        /// <returns>List&lt; SelectListItem  &gt;.</returns>
        List<SelectListItem> GetAllRoleForDropDown();

        /// <summary>
        /// Gets the role by role identifier.
        /// </summary>
        /// <param name="lgRoleId">The long role identifier.</param>
        /// <returns>Role  Model.</returns>
        RoleModel GetRoleByRoleId(long lgRoleId);

        /// <summary>
        /// Saves the role.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveRole(RoleModel objSave);

        /// <summary>
        /// Determines whether [is role exists] [the specified long role identifier].
        /// </summary>
        /// <param name="lgRoleId">The long role identifier.</param>
        /// <param name="strRoleName">Name of the string role.</param>
        /// <returns><c>true</c> if [is role exists] [the specified long role identifier]; otherwise, <c>false</c>.</returns>
        bool IsRoleExists(long lgRoleId, string strRoleName);

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="strRoleIdList">The string role identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteRole  Result.</returns>
        DeleteRoleResult DeleteRole(string strRoleIdList, long lgDeletedBy);

        /// <summary>
        /// Searches the role.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;SearchRole Result  &gt;.</returns>
        List<SearchRoleResult> SearchRole(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// GET the role permission by role identifier.
        /// </summary>
        /// <param name="lgRoleId">The long role identifier.</param>
        /// <returns>List&lt;GetPagePermissionResult   &gt;.</returns>
        List<GetPagePermissionResult> GerRolePermissionByRoleId(long lgRoleId);
    }
}
