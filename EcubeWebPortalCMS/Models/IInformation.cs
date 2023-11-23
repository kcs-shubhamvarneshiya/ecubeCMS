// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-13-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-13-2017
// ***********************************************************************
// <copyright file="iInformation.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>iInformation.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Interface iInformation.
    /// </summary>
    public interface IInformation
    {
        /// <summary>
        /// Gets the information list.
        /// </summary>
        /// <returns>List&lt;Information&gt; get information list.</returns>
        List<InformationModel> GetInformationList();

        /// <summary>
        /// Gets the information by identifier.
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <returns>InformationModel get information by identifier.</returns>
        InformationModel GetInformationById(int id);
        
        /// <summary>
        /// Searches the information.
        /// </summary>
        /// <param name="inRow">The in row Parameter.</param>
        /// <param name="inPage">The in page Parameter.</param>
        /// <param name="strSearch">The string search Parameter.</param>
        /// <param name="strSort">The string sort Parameter.</param>
        /// <returns>List&lt;SearchInformationResult&gt; search information.</returns>
        List<SearchInformationResult> SearchInformation(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Deletes the information.
        /// </summary>
        /// <param name="strInformationList">The string information list Parameter.</param>
        /// <param name="deletedBy">The deleted by Parameter.</param>
        /// <returns>DeleteInformationResult delete information.</returns>
        DeleteInformationResult DeleteInformation(string strInformationList, int deletedBy);

        /// <summary>
        /// Determines whether [is information exists] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <param name="informationName">Name of the information.</param>
        /// <returns><c>true</c> if [is information exists] [the specified identifier]; otherwise, <c>false</c>.</returns>
        bool IsInformationExists(int id, string informationName);

        /// <summary>
        /// Saves the information.
        /// </summary>
        /// <param name="objSave">The object save Parameter.</param>
        /// <returns>System save information.</returns>
        int SaveInformation(InformationModel objSave);
    }
}