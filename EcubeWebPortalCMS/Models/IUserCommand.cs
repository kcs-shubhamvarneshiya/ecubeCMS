// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="IUserCommand.cs" company="string.Empty">
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
    /// Interface IUserCommand.
    /// </summary>
    public interface IUserCommand
    {
        /// <summary>
        /// Gets all user for drop down.
        /// </summary>
        /// <returns>List&lt;SelectList Item &gt;.</returns>
        List<SelectListItem> GetAllUserForDropDown();

        /// <summary>
        /// Gets the user by user identifier.
        /// </summary>
        /// <param name="lgUserId">The long user identifier.</param>
        /// <returns>User Model.</returns>
        UserModel GetUserByUserId(long lgUserId);

        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveUser(UserModel objSave);

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="strUserId">The string user identifier.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteUser Result.</returns>
        DeleteUserResult DeleteUser(string strUserId, long lgDeletedBy);

        /// <summary>
        /// Validates the login.
        /// </summary>
        /// <param name="strUserName">Name of the string user.</param>
        /// <param name="strPassword">The string password.</param>
        /// <returns>User Model.</returns>
        UserModel ValidateLogin(string strUserName, string strPassword);

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="lgUserId">The long user identifier.</param>
        /// <param name="strUserPwd">The string user password.</param>
        void ChangePassword(long lgUserId, string strUserPwd);

        /// <summary>
        /// Determines whether [is user exists] [the specified long user identifier].
        /// </summary>
        /// <param name="lgUserId">The long user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns><c>true</c> if [is user exists] [the specified long user identifier]; otherwise, <c>false</c>.</returns>
        bool IsUserExists(long lgUserId, string userName);

        /// <summary>
        /// Searches the user.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;SearchUser Result &gt;.</returns>
        List<SearchUserResult> SearchUser(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Gets the user by email identifier.
        /// </summary>
        /// <param name="strEmailId">The string email identifier.</param>
        /// <returns>User Model.</returns>
        UserModel GetUserByEmailId(string strEmailId);

        UserModel CRMUserLogin(string userName,string password);

    }
}
