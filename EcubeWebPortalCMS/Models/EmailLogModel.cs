// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="EmailLogModel.cs" company="string.Empty">
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
    using System.Transactions;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;

    /// <summary>
    /// Class EmailLogModel.
    /// </summary>
    public partial class EmailLogModel
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
        public long? RelaventId { get; set; }

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
        /// Gets or sets the content of the mail.
        /// </summary>
        /// <value>The content of the mail.</value>
        public string MailContent { get; set; }

        /// <summary>
        /// Gets or sets the mail to.
        /// </summary>
        /// <value>The mail to.</value>
        public string MailTo { get; set; }

        /// <summary>
        /// Gets or sets the cc.
        /// </summary>
        /// <value>Parameter cc.</value>
        public string CC { get; set; }

        /// <summary>
        /// Gets or sets the BCC.
        /// </summary>
        /// <value>Parameter BCC.</value>
        public string BCC { get; set; }

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
        /// Gets or sets the email log list.
        /// </summary>
        /// <value>The email log list.</value>
        public List<EmailLogModel> EmailLogList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }
    }
}
