// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-15-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-18-2017
// ***********************************************************************
// <copyright file="IFacilityCategory.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>IFacilityCategory.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Interface IFacilityCategory.
    /// </summary>
    public interface IFacilityCategory
    {
        /// <summary>
        /// Gets the category list.
        /// </summary>
        /// <returns>List&lt;FacilityCategoryModel&gt; get category list.</returns>
        List<FacilityCategoryModel> GetCategoryList();

        /// <summary>
        /// Searches the facility category.
        /// </summary>
        /// <param name="inRow">The in row Parameter.</param>
        /// <param name="inPage">The in page Parameter.</param>
        /// <param name="strSearch">The string search Parameter.</param>
        /// <param name="strSort">The string sort Parameter.</param>
        /// <returns>List&lt;SearchFacilityCategoryResult&gt; search facility category.</returns>
        List<SearchFacilityCategoryResult> SearchFacilityCategory(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Gets the category by identifier.
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <returns>FacilityCategoryModel get category by identifier.</returns>
        FacilityCategoryModel GetCategoryById(int id);

        /// <summary>
        /// Determines whether [is category exists] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns><c>true</c> if [is category exists] [the specified identifier]; otherwise, <c>false</c>.</returns>
        bool IsCategoryExists(int id, string categoryName);

        /// <summary>
        /// Saves the category.
        /// </summary>
        /// <param name="objSave">The object save Parameter.</param>
        /// <returns>System save category.</returns>
        int SaveCategory(FacilityCategoryModel objSave);
        
        /// <summary>
        /// Searches the facility group.
        /// </summary>
        /// <param name="inRow">The in row Parameter.</param>
        /// <param name="inPage">The in page Parameter.</param>
        /// <param name="strSearch">The string search Parameter.</param>
        /// <param name="strSort">The string sort Parameter.</param>
        /// <returns>List&lt;SearchFacilityGroupResult&gt; search facility group.</returns>
        List<SearchFacilityGroupResult> SearchFacilityGroup(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Categories the list.
        /// </summary>
        /// <returns>List&lt;SelectListItem&gt; category list.</returns>
        List<SelectListItem> GetAllCategoryList();

        /// <summary>
        /// Gets the facility by identifier.
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <returns>FacilityGroupModel get facility by identifier.</returns>
        FacilityGroupModel GetFacilityById(int id);

        /// <summary>
        /// Saves the facility.
        /// </summary>
        /// <param name="objSave">The object save Parameter.</param>
        /// <returns>System save facility.</returns>
        int SaveFacility(FacilityGroupModel objSave);

        /// <summary>
        /// Deletes the category.
        /// </summary>
        /// <param name="strCategoryId">The string category identifier Parameter.</param>
        /// <param name="userId">The user identifier Parameter.</param>
        /// <returns>DeleteFacilityCategoryResult delete category.</returns>
        DeleteFacilityCategoryResult DeleteCategory(string strCategoryId, int userId);

        /// <summary>
        /// Deletes the facility.
        /// </summary>
        /// <param name="strFacilityId">The string facility identifier Parameter.</param>
        /// <param name="userId">The user identifier Parameter.</param>
        /// <returns>DeleteFacilityResult delete facility.</returns>
        DeleteFacilityResult DeleteFacility(string strFacilityId, int userId);
    }
}