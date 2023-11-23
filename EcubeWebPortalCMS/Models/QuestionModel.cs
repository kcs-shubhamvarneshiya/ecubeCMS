// ***********************************************************************
// Assembly         : MagnitudeGold
// Author           : Ghanshyam Dhanani
// Created          : 06-10-2015
//
// Last Modified By : Ghanshyam Dhanani
// Last Modified On : 09-07-2016
// ***********************************************************************
// <copyright file="QuestionModel.cs" company="KCSPL">
//     Copyright ©  2015
// </copyright>
// <summary>Question Model</summary>
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
    /// Class QuestionModel.
    /// </summary>
    public class QuestionModel
    {
        public long QuestionnaireId { get; set; }

        /// <summary>
        /// Gets or sets the LG question identifier.
        /// </summary>
        /// <value>The LG question identifier.</value>
        public long QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the in position.
        /// </summary>
        /// <value>The in position.</value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the in question type identifier.
        /// </summary>
        /// <value>The in question type identifier.</value>
        public int QuestionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the string question title.
        /// </summary>
        /// <value>The string question title.</value>
        public string QuestionTitle { get; set; }

        /// <summary>
        /// Gets or sets the short name of the string.
        /// </summary>
        /// <value>The short name of the string.</value>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL required].
        /// </summary>
        /// <value><c>true</c> if [BL required]; otherwise, <c>false</c>.</value>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL signature].
        /// </summary>
        /// <value><c>true</c> if [BL required]; otherwise, <c>false</c>.</value>
        public bool IsSignature { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL display in summary].
        /// </summary>
        /// <value><c>true</c> if [BL display in summary]; otherwise, <c>false</c>.</value>
        public bool IsDisplayInSummary { get; set; }

        /// <summary>
        /// Gets or sets the repetitive questions group no.
        /// </summary>
        /// <value>The repetitive questions group no.</value>
        public int RepetitiveQuestionsGroupNo { get; set; }

        /// <summary>
        /// Gets or sets the name of the repetitive questions group.
        /// </summary>
        /// <value>The name of the repetitive questions group.</value>
        public string RepetitiveQuestionsGroupName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL display in detail].
        /// </summary>
        /// <value><c>true</c> if [BL display in detail]; otherwise, <c>false</c>.</value>
        public bool IsDisplayInDetail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL is active].
        /// </summary>
        /// <value><c>true</c> if [BL is active]; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of the in.
        /// </summary>
        /// <value>The maximum length of the in.</value>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the string hint.
        /// </summary>
        /// <value>The string hint.</value>
        public string Hint { get; set; }

        /// <summary>
        /// Gets or sets the in escalation regex.
        /// </summary>
        /// <value>The in escalation regex.</value>
        public int EscalationRegex { get; set; }

        /// <summary>
        /// Gets or sets the display type of the string option.
        /// </summary>
        /// <value>The display type of the string option.</value>
        public string OptionDisplayType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL is title bold].
        /// </summary>
        /// <value><c>true</c> if [BL is title bold]; otherwise, <c>false</c>.</value>
        public bool IsTitleBold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL is title italic].
        /// </summary>
        /// <value><c>true</c> if [BL is title italic]; otherwise, <c>false</c>.</value>
        public bool IsTitleItalic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL is title underline].
        /// </summary>
        /// <value><c>true</c> if [BL is title underline]; otherwise, <c>false</c>.</value>
        public bool IsTitleUnderline { get; set; }

        /// <summary>
        /// Gets or sets the color of the string title text.
        /// </summary>
        /// <value>The color of the string title text.</value>
        public string TitleTextColor { get; set; }

        /// <summary>
        /// Gets or sets the LG contact question identifier.
        /// </summary>
        /// <value>The LG contact question identifier.</value>
        public long? ContactQuestionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the string table group.
        /// </summary>
        /// <value>The name of the string table group.</value>
        public string TableGroupName { get; set; }

        /// <summary>
        /// Gets or sets the in margin.
        /// </summary>
        /// <value>The in margin.</value>
        public int Margin { get; set; }

        /// <summary>
        /// Gets or sets the size of the in font.
        /// </summary>
        /// <value>The size of the in font.</value>
        public int FontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL is group field].
        /// </summary>
        /// <value><c>true</c> if [BL is group field]; otherwise, <c>false</c>.</value>
        public bool IsGroupField { get; set; }

        /// <summary>
        /// Gets or sets the string image path.
        /// </summary>
        /// <value>The string image path.</value>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the LG escalation value.
        /// </summary>
        /// <value>The LG escalation value.</value>
        public long EscalationValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL display in graphs].
        /// </summary>
        /// <value><c>true</c> if [BL display in graphs]; otherwise, <c>false</c>.</value>
        public bool DisplayInGraphs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL display in table view].
        /// </summary>
        /// <value><c>true</c> if [BL display in table view]; otherwise, <c>false</c>.</value>
        public bool DisplayInTableView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL is comment compulsory].
        /// </summary>
        /// <value><c>true</c> if [BL is comment compulsory]; otherwise, <c>false</c>.</value>
        public bool IsCommentCompulsory { get; set; }

        /// <summary>
        /// Gets or sets the LST option.
        /// </summary>
        /// <value>The LST option.</value>
        public List<OptionsModel> LstOption { get; set; }

        /// <summary>
        /// Gets or sets the LG multiple routing value.
        /// </summary>
        /// <value>The LG multiple routing value.</value>
        public long MultipleRoutingValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL is anonymous].
        /// </summary>
        /// <value><c>true</c> if [BL is anonymous]; otherwise, <c>false</c>.</value>
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is decimal.
        /// </summary>
        /// <value><c>true</c> if this instance is decimal; otherwise, <c>false</c>.</value>
        public bool AllowDecimal { get; set; }

        /// <summary>
        /// Gets or sets the image height.
        /// </summary>
        /// <value>
        /// The image height.
        /// </value>
        public string Imageheight { get; set; }

        /// <summary>
        /// Gets or sets the image width.
        /// </summary>
        /// <value>
        /// The image width.
        /// </value>
        public string Imagewidth { get; set; }

        /// <summary>
        /// Gets or sets the image align.
        /// </summary>
        /// <value>
        /// The image align.
        /// </value>
        public string Imagealign { get; set; }

        /// <summary>
        /// Gets or sets the calculation option identifier.
        /// </summary>
        /// <value>
        /// The calculation option identifier.
        /// </value>
        public int CalculationOptionId { get; set; }

        /// <summary>
        /// Gets or sets the summary option identifier.
        /// </summary>
        /// <value>
        /// The summary option identifier.
        /// </value>
        public int SummaryOptionId { get; set; }

        public bool IsDelete { get; set; }
    }
}