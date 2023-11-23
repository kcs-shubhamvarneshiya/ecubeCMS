// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-12-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-12-2017
// ***********************************************************************
// <copyright file="MessageType.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>MessageType.cs</summary>
// ***********************************************************************

/// <summary>
/// The Common namespace.
/// </summary>
namespace EcubeWebPortalCMS.Common
{
    /// <summary>
    /// Enum MessageType.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// An enum constant representing the success option..
        /// </summary>
        Success = 1,

        /// <summary>
        /// An enum constant representing the fail option..
        /// </summary>
        Fail = 2,

        /// <summary>
        /// An enum constant representing the delete success option..
        /// </summary>
        DeleteSucess = 3,

        /// <summary>
        /// An enum constant representing the delete fail option..
        /// </summary>
        DeleteFail = 4,

        /// <summary>
        /// An enum constant representing the delete partial option..
        /// </summary>
        DeletePartial = 5,

        /// <summary>
        /// An enum constant representing the already exist option..
        /// </summary>
        AlreadyExist = 6,

        /// <summary>
        /// An enum constant representing the input required option..
        /// </summary>
        InputRequired = 7,

        /// <summary>
        /// An enum constant representing the select required option..
        /// </summary>
        SelectRequired = 8,

        /// <summary>
        /// An enum constant representing the record in grid required option..
        /// </summary>
        RecordInGridRequired = 9,

        /// <summary>
        /// An enum constant representing the file upload required option..
        /// </summary>
        UploadRequired = 10,

        /// <summary>
        /// An enum constant representing the static message required option..
        /// </summary>
        StaticMessage = 11,

        /// <summary>
        /// An enum constant representing the success option..
        /// </summary>
        UpdateSuccess = 12,
        /// <summary>
        /// An enum constant representing the DuplicateRows option..
        /// </summary>
        DuplicateRows = 13,
        /// <summary>
        /// An enum constant representing the delete menu when 1 sub menu option..
        /// </summary>
        DeleteMenuSubMenu = 14,
    }
}