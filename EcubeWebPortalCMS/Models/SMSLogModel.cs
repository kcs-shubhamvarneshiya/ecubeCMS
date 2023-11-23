// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="SMSLogModel.cs" company="string.Empty">
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
   using System.Web.Mvc;

    /// <summary>
    /// Class SMSLogModel.
    /// </summary>
    public partial class SMSLogModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the relevant identifier.
        /// </summary>
        /// <value>The relevant identifier.</value>
        public int? RelaventId { get; set; }

        /// <summary>
        /// Gets or sets the module identifier.
        /// </summary>
        /// <value>The module identifier.</value>
        public long ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the module list.
        /// </summary>
        /// <value>The module list.</value>
        public List<SelectListItem> ModuleList { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the mobile no.
        /// </summary>
        /// <value>The mobile no.</value>
        public string MobileNo { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>Parameter text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the sent on.
        /// </summary>
        /// <value>The sent on.</value>
        public DateTime SentOn { get; set; }

        /// <summary>
        /// Gets or sets the string sent on.
        /// </summary>
        /// <value>The string sent on.</value>
        public string StrSentOn { get; set; }

        /// <summary>
        /// Gets or sets the SMS log list.
        /// </summary>
        /// <value>The SMS log list.</value>
        public List<SMSLogModel> SMSLogList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }
    }
}
