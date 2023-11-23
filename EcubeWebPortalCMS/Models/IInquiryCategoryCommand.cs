// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : Savan Marakna
// Created          : 11-10-2023
//

// ***********************************************************************
// <copyright file="IInquiryCategoryCommand.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Models
{
    public interface IInquiryCategoryCommand
    {
        /// <summary>
        /// Gets all Inquiry category for drop down.
        /// </summary>
        /// <returns>List SelectList Item.</returns>
        List<SelectListItem> GetAllInquiryCategoryForDropDown();

        /// <summary>
        /// Gets the inquiry category by inquiry category identifier.
        /// </summary>
        /// <param name="lgInquiryCategoryId">The LONG inquiry category identifier.</param>
        /// <returns>Inquiry Category Model.</returns>
        InquiryCategoryModel GetInquiryCategoryByInquiryCategoryId(int lgInquiryCategoryId);

        /// <summary>
        /// Searches the Inquiry category.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List MS_SearchInquiry CategoryResult.</returns>
        List<MS_SearchInquiryCategoryResult> SearchInquiryCategory(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Determines whether [is Inquiry category exists] [the specified LONG Inquiry category identifier].
        /// </summary>
        /// <param name="lgInquiryCategoryId">The LONG Inquiry category identifier.</param>
        /// <param name="strInquiryCategoryName">Name of the string Inquiry category.</param>
        /// <returns><c>true</c> if [is Inquiry category exists] [the specified LONG Inquiry category identifier]; otherwise, <c>false</c>.</returns>
        bool IsInquiryCategoryExists(long lgInquiryCategoryId, string strInquiryCategoryName, int SeqNo);

        bool GetDuplicateSeq(long lgInquiryCategoryId, string strInquiryCategoryName, int SeqNo);

        /// <summary>
        /// Saves the Inquiry category.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        long SaveInquiryCategory(InquiryCategoryModel objSave);

        /// <summary>
        /// Deletes the Inquiry category.
        /// </summary>
        /// <param name="strInquiryCategoryList">The string Inquiry category list.</param>
        /// <param name="lgDeletedBy">The LONG deleted by.</param>
        /// <returns>MS_Delete InquiryCategory  Result.</returns>
        DeleteInquiryCategoryResult DeleteInquiryCategory(string strEventCategoryList, long lgDeletedBy);
    }
}
