// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="ErrorLogModel.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
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
    /// Class ErrorLogModel.
    /// </summary>
    public partial class ErrorLogModel
    {
        /// <summary>
        /// Gets or sets the error identifier.
        /// </summary>
        /// <value>The error identifier.</value>
        public long ErrorId { get; set; }

        /// <summary>
        /// Gets or sets the module identifier.
        /// </summary>
        /// <value>The module identifier.</value>
        public long ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the module DESC.
        /// </summary>
        /// <value>The module DESC.</value>
        public string ModuleDesc { get; set; }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        /// <value>The name of the page.</value>
        public string PageName { get; set; }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public string ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        /// <value>The error details.</value>
        public string ErrorDetails { get; set; }

        /// <summary>
        /// Gets or sets the error date.
        /// </summary>
        /// <value>The error date.</value>
        public DateTime ErrorDate { get; set; }

        /// <summary>
        /// Gets or sets the string error date.
        /// </summary>
        /// <value>The string error date.</value>
        public string StrErrorDate { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        /// <value>The solution.</value>
        public string Solution { get; set; }

        /// <summary>
        /// Gets or sets the error log list.
        /// </summary>
        /// <value>The error log list.</value>
        public List<ErrorLogModel> ErrorLogList { get; set; }
    }
}