// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="UserCommand.cs" company="string.Empty">
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
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class UserCommand.
    /// </summary>
    public partial class UserCommand : IUserCommand, IDisposable
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Context for the object data.
        /// </summary>
        private readonly UserDataContext objDataContext = new UserDataContext();

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool disposed = false;

        

        /// <summary>
        /// Gets all user for drop down.
        /// </summary>
        /// <returns>List&lt;SelectList Item &gt;.</returns>
        public List<SelectListItem> GetAllUserForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objUserList = new List<SelectListItem>();
            try
            {
                objUserList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                List<GetUserAllResult> objUserResultList = this.objDataContext.GetUserAll().ToList();
                if (objUserResultList.Count > 0)
                {
                    foreach (var item in objUserResultList)
                    {
                        objUserList.Add(new SelectListItem { Text = item.UserName, Value = item.Id.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objUserList;
        }

        /// <summary>
        /// Gets the user by user identifier.
        /// </summary>
        /// <param name="lgUserId">The long user identifier.</param>
        /// <returns>User Model.</returns>
        public UserModel GetUserByUserId(long lgUserId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            UserModel objUser = new UserModel();
            GetUserByIdResult item = this.objDataContext.GetUserById(lgUserId).FirstOrDefault();
            try
            {
                if (item != null)
                {
                    objUser.Id = item.Id;
                    objUser.FirstName = item.FirstName;
                    objUser.SurName = item.SurName;
                    objUser.MobileNo = item.MobileNo;
                    objUser.EmailID = item.EmailID;
                    objUser.UserName = item.UserName;
                    objUser.Password = item.Password.DecryptString();
                    objUser.Address = item.Address;
                    objUser.RoleId = item.RoleId;
                    objUser.IsActive = item.IsActive;
                    objUser.IsLogin = item.IsLogin;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objUser;
        }

        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveUser(UserModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            System.Data.Common.DbTransaction tran = null;
            try
            {
                this.objDataContext.Connection.Open();
                tran = this.objDataContext.Connection.BeginTransaction();
                this.objDataContext.Transaction = tran;
                objSave.Password = objSave.Password.EncryptString();
                

                var result = this.objDataContext.InsertOrUpdateUser(objSave.Id, objSave.FirstName == null ? DBNull.Value.ToString() : objSave.FirstName, objSave.SurName == null ? DBNull.Value.ToString() :objSave.SurName, objSave.MobileNo == null ? DBNull.Value.ToString() : objSave.MobileNo, objSave.EmailID == null ? DBNull.Value.ToString() : objSave.EmailID, objSave.UserName, objSave.Password, objSave.Address == null ? DBNull.Value.ToString() :objSave.Address, objSave.RoleId, objSave.IsActive, objSave.IsLogin, MySession.Current.UserId, PageMaster.User).FirstOrDefault();
                if (result != null)
                {
                    tran.Commit();
                    return result.InsertedId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                tran.Rollback();
                return 0;
            }
            finally
            {
                if (this.objDataContext.Connection != null && this.objDataContext.Connection.State == System.Data.ConnectionState.Open)
                {
                    this.objDataContext.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="strUserId">The string user identifier.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteUser Result.</returns>
        public DeleteUserResult DeleteUser(string strUserId, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            System.Data.Common.DbTransaction tran = null;
            try
            {
                this.objDataContext.Connection.Open();
                tran = this.objDataContext.Connection.BeginTransaction();
                this.objDataContext.Transaction = tran;
                DeleteUserResult result = this.objDataContext.DeleteUser(strUserId, lgDeletedBy, PageMaster.User).FirstOrDefault();
                if (result != null)
                {
                    tran.Commit();
                }

                return result;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                tran.Rollback();
                return null;
            }
            finally
            {
                if (this.objDataContext.Connection != null && this.objDataContext.Connection.State == System.Data.ConnectionState.Open)
                {
                    this.objDataContext.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Validates the login.
        /// </summary>
        /// <param name="strUserName">Name of the string user.</param>
        /// <param name="strPassword">The string password.</param>
        /// <returns>User Model.</returns>
        public UserModel ValidateLogin(string strUserName, string strPassword)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                User objUser = this.objDataContext.Users.Where(n => n.UserName == strUserName && n.Password == strPassword && n.IsActive == true && n.IsDeleted == false).FirstOrDefault();
                if (objUser != null)
                {
                    UserModel objUserModel = new UserModel();
                    objUserModel.Id = objUser.Id;
                    objUserModel.FirstName = objUser.FirstName;
                    objUserModel.SurName = objUser.SurName;
                    objUserModel.MobileNo = objUser.MobileNo;
                    objUserModel.EmailID = objUser.EmailID;
                    objUserModel.UserName = objUser.UserName;
                    objUserModel.Password = objUser.Password.DecryptString();
                    objUserModel.Address = objUser.Address;
                    objUserModel.RoleId = objUser.RoleId;
                    objUserModel.IsActive = objUser.IsActive;
                    objUserModel.IsLogin = objUser.IsLogin;
                    return objUserModel;
                }

                return null;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="lgUserId">The long user identifier.</param>
        /// <param name="strUserPwd">The string user password.</param>
        public void ChangePassword(long lgUserId, string strUserPwd)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                UserModel objUserMaster = this.GetUserByUserId(lgUserId);
                if (objUserMaster != null)
                {
                    objUserMaster.Password = strUserPwd;
                }

                this.SaveUser(objUserMaster);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        /// <summary>
        /// Determines whether [is user exists] [the specified long user identifier].
        /// </summary>
        /// <param name="lgUserId">The long user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns><c>true</c> if [is user exists] [the specified long user identifier]; otherwise, <c>false</c>.</returns>
        public bool IsUserExists(long lgUserId, string userName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (this.objDataContext.Users.Where(x => x.Id != lgUserId && x.UserName == userName).Count() > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Searches the user.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;SearchUser Result &gt;.</returns>
        public List<SearchUserResult> SearchUser(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.objDataContext.SearchUser(inRow, inPage, strSearch, strSort).ToList();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Gets the user by email identifier.
        /// </summary>
        /// <param name="strEmailId">The string email identifier.</param>
        /// <returns>User Model.</returns>
        public UserModel GetUserByEmailId(string strEmailId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            UserModel objUser = new UserModel();
            try
            {
                GetUserByEmailIdResult item = this.objDataContext.GetUserByEmailId(strEmailId).FirstOrDefault();
                if (item != null)
                {
                    objUser.Id = item.Id;
                    objUser.EmailID = item.EmailID;
                    objUser.FirstName = item.FirstName;
                    objUser.SurName = item.SurName;
                    objUser.MobileNo = item.MobileNo;
                    objUser.Password = item.Password;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objUser;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
                ////ErrorLog.Write(string.Format(this.GetType().Name, "Dispose"), ex, LogType.Critical);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        if (this.objDataContext != null)
                        {
                            this.objDataContext.Dispose();
                        }
                    }

                    this.disposed = true;
                }
            }
            catch (Exception)
            {
                ////ErrorLog.Write(string.Format(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name), ex, LogType.Critical);
            }
        }

        public UserModel CRMUserLogin(string userName, string password)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            UserModel objUser = new UserModel();
            CRMUserLoginResult item = this.objDataContext.CRMUserLogin(userName, password).FirstOrDefault();
            try
            {
                if (item != null)
                {
                    objUser.Id = item.Id;
                    objUser.UserName = item.UserName;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objUser;
        }

        public GetPageRightsByUserIdResult GetPageRights(int userId, string pageDisplayName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            GetPageRightsByUserIdResult item = new GetPageRightsByUserIdResult();

            try
            {
                item = this.objDataContext.GetPageRightsByUserId(userId, pageDisplayName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return item;
        }
    }
}