// ***********************************************************************
// Assembly         : MagnitudeGold
// Author           : Ghanshyam Dhanani
// Created          : 06-10-2015
//
// Last Modified By : Ghanshyam Dhanani
// Last Modified On : 09-24-2016
// ***********************************************************************
// <copyright file="ClsQuestionnaire.cs" company="KCSPL">
//     Copyright ©  2015
// </copyright>
// <summary>Cls Questionnaire</summary>
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
    /// Class CLSQuestionnaire.
    /// </summary>
    public partial class QuestionnaireModel
    {
        /// <summary>
        /// The object data context variable.
        /// </summary>
        //private QuestionnaireDataContext objDataContext = null;

        /// <summary>
        /// Gets or sets the LONG identifier.
        /// </summary>
        /// <value>The LONG identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the string questionnaire title.
        /// </summary>
        /// <value>The string questionnaire title.</value>
        public string QuestionnaireTitle { get; set; }

        /// <summary>
        /// Gets or sets the type of the string questionnaire.
        /// </summary>
        /// <value>The type of the string questionnaire.</value>
        public string QuestionnaireType { get; set; }

        /// <summary>
        /// Gets or sets the string description.
        /// </summary>
        /// <value>The string description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the LST seen client.
        /// </summary>
        /// <value>The LST seen client.</value>
        public List<SelectListItem> lstSeenClient { get; set; }

        /// <summary>
        /// Gets or sets the LST seen client questions.
        /// </summary>
        /// <value>The LST seen client questions.</value>
        public List<SelectListItem> lstSeenClientQuestions { get; set; }

        /// <summary>
        /// Gets or sets the LONG contact identifier.
        /// </summary>
        /// <value>The LONG contact identifier.</value>
        public long ContactId { get; set; }

        /// <summary>
        /// Gets or sets the LST contact.
        /// </summary>
        /// <value>The LST contact.</value>
        public List<SelectListItem> LstContact { get; set; }

        /// <summary>
        /// Gets or sets the LONG contact question identifier.
        /// </summary>
        /// <value>The LONG contact question identifier.</value>
        public long ContactQuestionId { get; set; }

        /// <summary>
        /// Gets or sets the LST contact questions.
        /// </summary>
        /// <value>The LST contact questions.</value>
        public List<SelectListItem> LstContactQuestions { get; set; }

        /// <summary>
        /// Gets or sets the LONG seen client identifier.
        /// </summary>
        /// <value>The LONG seen client identifier.</value>
        public long SeenClientId { get; set; }

        /// <summary>
        /// Gets or sets the LONG seen client question identifier.
        /// </summary>
        /// <value>The LONG seen client question identifier.</value>
        public long SeenClientQuestionId { get; set; }

        /// <summary>
        /// Gets or sets the in question type identifier.
        /// </summary>
        /// <value>The in question type identifier.</value>
        public int QuestionTypeId { get; set; }

        public int? RecursionTypeId { get; set; }
        public DateTime? RecursionFrom { get; set; }
        public DateTime? RecursionTo { get; set; }
        public bool? RecursionSMS { get; set; }
        public bool? RecursionEmail { get; set; }
        public bool IsRecursionCreated { get; set; }

        /// <summary>
        /// Gets or sets the type of the LST question.
        /// </summary>
        /// <value>The type of the LST question.</value>
        public List<SelectListItem> LstQuestionType { get; set; }

        /// <summary>
        /// Gets or sets the list questionnaire.
        /// </summary>
        /// <value>The list questionnaire.</value>
        public List<QuestionnaireModel> LstQuestionnaire { get; set; }

        /// <summary>
        /// Gets or sets the LST questionnaire question.
        /// </summary>
        /// <value>The LST questionnaire question.</value>
        public List<GetQuestionsByQuestionnaireIdResult> LstQuestionnaireQuestion { get; set; }

        /// <summary>
        /// Gets or sets the LST questionnaire options.
        /// </summary>
        /// <value>The LST questionnaire options.</value>
        public List<GetOptionsByQuestionnaireIdResult> LstQuestionnaireOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDNI frame].
        /// </summary>
        /// <value><c>true</c> if [HDNI frame]; otherwise, <c>false</c>.</value>
        public bool hdniFrame { get; set; }

        /// <summary>
        /// Gets or sets the string deleted question identifier.
        /// </summary>
        /// <value>The string deleted question identifier.</value>
        public string DeletedQuestionId { get; set; }

        /// <summary>
        /// Gets or sets the type of the string compare.
        /// </summary>
        /// <value>The type of the string compare.</value>
        public string strCompareType { get; set; }

        /// <summary>
        /// Gets or sets the DT last test date.
        /// </summary>
        /// <value>The DT last test date.</value>
        public DateTime? dtLastTestDate { get; set; }

        /// <summary>
        /// Gets or sets the string last test date.
        /// </summary>
        /// <value>The string last test date.</value>
        public string strLastTestDate { get; set; }

        /// <summary>
        /// Gets or sets the string test time.
        /// </summary>
        /// <value>The string test time.</value>
        public string strTestTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL is active].
        /// </summary>
        /// <value><c>true</c> if [BL is active]; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }
    }
}