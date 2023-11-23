// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="IIPAddressCommand.cs" company="string.Empty">
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
    /// Interface IIPAddressCommand.
    /// </summary>
    public interface IIPAddressCommand
    {
        /// <summary>
        /// Gets all IP   address for drop down.
        /// </summary>
        /// <returns>List; SelectList Item;.</returns>
        List<SelectListItem> GetAllIPAddressForDropDown();

        /// <summary>
        /// Gets the IP   address by IP   address identifier.
        /// </summary>
        /// <param name="lgIPAddressId">The long IP   address identifier.</param>
        /// <returns>IP Address  Model.</returns>
        IPAddressModel GetIPAddressByIPAddressId(long lgIPAddressId);

        /// <summary>
        /// Saves the IP   address.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        long SaveIPAddress(IPAddressModel objSave);

        /// <summary>
        /// Deletes the IP   address.
        /// </summary>
        /// <param name="strIPAddressIdList">The string IP    address identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>Delete IPAddress  Result.</returns>
        DeleteIPAddressResult DeleteIPAddress(string strIPAddressIdList, long lgDeletedBy);

        /// <summary>
        /// Determines whether [is IP   address exists] [the specified long IP   address identifier].
        /// </summary>
        /// <param name="lgIPAddressId">The long IP   address identifier.</param>
        /// <param name="strIPAddressName">Name of the string IP   address.</param>
        /// <returns><c>true</c> if [is IP   address exists] [the specified long IP   address identifier]; otherwise, <c>false</c>.</returns>
        bool IsIPAddressExists(long lgIPAddressId, string strIPAddressName);

        /// <summary>
        /// Searches the IP   address.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;Search IPAddress Result  &gt;.</returns>
        List<SearchIPAddressResult> SearchIPAddress(int inRow, int inPage, string strSearch, string strSort);
    }
}
