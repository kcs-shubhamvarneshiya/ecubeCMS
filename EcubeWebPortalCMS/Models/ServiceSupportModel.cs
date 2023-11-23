// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-10-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-10-2016
// ***********************************************************************
// <copyright file="ServiceSupportModel.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>ServiceSupportModel</summary>
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
    /// Class ServiceSupportModel.
    /// </summary>
    public class ServiceSupportModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The Member identifier.</value>
        public decimal? MemberId { get; set; }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>The name of the member.</value>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets the service support type identifier.
        /// </summary>
        /// <value>The service support type identifier.</value>
        public int? ServiceSupportTypeId { get; set; }

        /// <summary>
        /// Gets or sets the support type list.
        /// </summary>
        /// <value>The support type list.</value>
        public List<SelectListItem> SupportTypeList { get; set; }

        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>The status identifier.</value>
        public int? StatusId { get; set; }

        /// <summary>
        /// Gets or sets the status list.
        /// </summary>
        /// <value>The status list.</value>
        public List<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the response by.
        /// </summary>
        /// <value>The response by.</value>
        public string ResponseBy { get; set; }

        /// <summary>
        /// Gets or sets the status DESC.
        /// </summary>
        /// <value>The status DESC.</value>
        public string StatusDesc { get; set; }

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the member code.
        /// </summary>
        /// <value>The member code.</value>
        public string MemberCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        public string CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }
    }
}