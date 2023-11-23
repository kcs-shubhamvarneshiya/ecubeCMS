// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-21-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-22-2016
// ***********************************************************************
// <copyright file="IDocumentLibraryCommand.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>IDocumentLibraryCommand</summary>
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
    /// Interface IDocumentLibraryCommand.
    /// </summary>
    public interface IDocumentLibraryCommand
    {
        /// <summary>
        /// Searches the document.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;CRM Search Document Result&gt;.</returns>
        List<CRMSearchDocumentResult> SearchDocument(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Saves the document library.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        long SaveDocumentLibrary(DocumentLibraryModel objSave);

        /// <summary>
        /// Gets the document by identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>Document Library Model.</returns>
        DocumentLibraryModel GetDocumentById(long documentId);

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="strSupportIdList">The string support identifier list.</param>
        /// <param name="lgDeletedBy">The deleted by.</param>
        /// <returns>CRMDelete Document Result.</returns>
        CRMDeleteDocumentResult DeleteDocument(string strSupportIdList, int lgDeletedBy);

        /// <summary>
        /// Determines whether [is type name exists] [the specified name identifier].
        /// </summary>
        /// <param name="lgNameId">The name identifier.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns><c>true</c> if [is type name exists] [the specified name identifier]; otherwise, <c>false</c>.</returns>
        bool IsTitleExists(long lgNameId, string typeName);
    }
}
