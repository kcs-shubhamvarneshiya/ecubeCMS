// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-15-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="INotification.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>INotification.cs</summary>
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
    /// Interface INotification.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Gets the notification list.
        /// </summary>
        /// <returns>List&lt;NotificationModel&gt; get notification list.</returns>
        List<NotificationModel> GetNotificationList();

        /// <summary>
        /// Searches the notification.
        /// </summary>
        /// <param name="inRow">The in row Parameter.</param>
        /// <param name="inPage">The in page Parameter.</param>
        /// <param name="strSearch">The string search Parameter.</param>
        /// <param name="strSort">The string sort Parameter.</param>
        /// <returns>List&lt;SearchNotificationResult&gt; search notification.</returns>
        List<SearchNotificationResult> SearchNotification(int inRow, int inPage, string strSearch, string strSort);

        /// <summary>
        /// Saves the notification.
        /// </summary>
        /// <param name="objSave">The object save Parameter.</param>
        /// <returns>System save notification.</returns>
        int SaveNotification(NotificationModel objSave);
    }
}