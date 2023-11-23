// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-20-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-20-2017
// ***********************************************************************
// <copyright file="PostCommand.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>PostCommand.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Transactions;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class PostCommand.
    /// </summary>
    public class PostCommand : IPost
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private PostDataContext objDataContext = new PostDataContext();

        /// <summary>
        /// Gets the post by identifier.
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <returns>PostModel get post by identifier.</returns>
        public PostModel GetPostById(int id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            PostModel postById = new PostModel();
            try
            {
                using (this.objDataContext = new PostDataContext())
                {
                    postById = (from c in this.objDataContext.Posts
                                where c.IsDeleted == false && c.Id == id
                                select new PostModel
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Description = c.PostDescription,
                                    Image1 = c.Image1,
                                    Image2 = c.Image2,
                                    Image3 = c.Image3,
                                    Image4 = c.Image4,
                                    Image5 = c.Image5
                                }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }

            return postById;
        }

        /// <summary>
        /// Searches the post.
        /// </summary>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="page">The page Parameter.</param>
        /// <param name="search">The search Parameter.</param>
        /// <param name="p">The p Parameter.</param>
        /// <returns>List&lt;SearchPostResult&gt; search post.</returns>
        public List<SearchPostResult> SearchPost(int rows, int page, string search, string p)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new PostDataContext())
                {
                    List<SearchPostResult> objSearchPostList = this.objDataContext.SearchPost(rows, page, search, p).ToList();
                    return objSearchPostList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Deletes the post.
        /// </summary>
        /// <param name="strPostId">The string post identifier Parameter.</param>
        /// <param name="userId">The user identifier Parameter.</param>
        /// <returns>DeletePostResult delete post.</returns>
        public DeletePostResult DeletePost(string strPostId, int userId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DeletePostResult result = new DeletePostResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new PostDataContext())
                    {
                        result = this.objDataContext.DeletePost(strPostId, userId, PageMaster.Post).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is post exists] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier Parameter.</param>
        /// <param name="name">The name Parameter.</param>
        /// <returns><c>true</c> if [is post exists] [the specified identifier]; otherwise, <c>false</c>.</returns>
        public bool IsPostExists(int id, string name)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new PostDataContext())
                {
                    if (this.objDataContext.Posts.Where(x => x.Id != id && x.Name == name && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Saves the Post.
        /// </summary>
        /// <param name="objSave">The object save Parameter.</param>
        /// <returns>System save Post.</returns>
        public int SavePost(PostModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int postId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new PostDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdatePost(objSave.Id, objSave.Name, objSave.Description, objSave.Image1, objSave.Image2, objSave.Image3, objSave.Image4, objSave.Image5, MySession.Current.UserId, PageMaster.Post).FirstOrDefault();
                        if (result != null)
                        {
                            postId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return postId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }
    }
}