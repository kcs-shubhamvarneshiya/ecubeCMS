// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-20-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-20-2017
// ***********************************************************************
// <copyright file="IPost.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>IPost.cs</summary>
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
    /// Interface IPost.
    /// </summary>
    public interface IPost
    {
        /// <summary>
        /// Deletes the post.
        /// </summary>
        /// <param name="strPostId">The string post identifier Parameter.</param>
        /// <param name="userId">The user identifier Parameter.</param>
        /// <returns>DeletePostResult delete post.</returns>
        DeletePostResult DeletePost(string strPostId, int userId);

        /// <summary>
        /// Determines whether [is post exists] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <param name="name">The name Parameter.</param>
        /// <returns><c>true</c> if [is post exists] [the specified identifier]; otherwise, <c>false</c>.</returns>
        bool IsPostExists(int id, string name);

        /// <summary>
        /// Gets the post by identifier.
        /// </summary>
        /// <param name="postId">The post identifier Parameter.</param>
        /// <returns>PostModel get post by identifier.</returns>
        PostModel GetPostById(int postId);

        /// <summary>
        /// Searches the post.
        /// </summary>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="page">The page Parameter.</param>
        /// <param name="search">The search Parameter.</param>
        /// <param name="p">The p Parameter.</param>
        /// <returns>List&lt;SearchPostResult&gt; search post.</returns>
        List<SearchPostResult> SearchPost(int rows, int page, string search, string p);

        /// <summary>
        /// Saves the post.
        /// </summary>
        /// <param name="objPost">The object post Parameter.</param>
        /// <returns>System save post.</returns>
        int SavePost(PostModel objPost);
    }
}