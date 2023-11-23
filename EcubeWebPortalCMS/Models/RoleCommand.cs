// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="RoleCommand.cs" company="string.Empty">
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
    using System.Transactions;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class RoleCommand.
    /// </summary>
    public partial class RoleCommand : IRoleCommand, IDisposable
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Context for the object data.
        /// </summary>
        private readonly RoleDataContext objDataContext = new RoleDataContext();

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Get All RoleData for DropDown.
        /// </summary>
        /// <returns>List&lt;SelectListItem  &gt;.</returns>
        public List<SelectListItem> GetAllRoleForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objRoleList = new List<SelectListItem>();
            try
            {
                objRoleList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                List<GetRoleAllResult> objRoleResultList = this.objDataContext.GetRoleAll().ToList();
                if (objRoleResultList.Count > 0)
                {
                    foreach (var item in objRoleResultList)
                    {
                        objRoleList.Add(new SelectListItem { Text = item.RoleName, Value = item.Id.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objRoleList;
        }

        /// <summary>
        /// Get RoleData By Id.
        /// </summary>
        /// <param name="lgRoleId">The long role identifier.</param>
        /// <returns>Role Model.</returns>
        public RoleModel GetRoleByRoleId(long lgRoleId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            RoleModel objRole = new RoleModel();
            GetRoleByIdResult item = this.objDataContext.GetRoleById(lgRoleId).FirstOrDefault();
            try
            {
                if (item != null)
                {
                    objRole.Id = item.Id;
                    objRole.RoleName = item.RoleName;
                    objRole.Description = item.Description;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objRole;
        }

        /// <summary>
        /// Save RoleData.
        /// </summary>
        /// <param name="objSave">Parameter OBJ Save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveRole(RoleModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                long roleId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext)
                    {
                        var result = this.objDataContext.InsertOrUpdateRole(objSave.Id, objSave.RoleName, objSave.Description, MySession.Current.UserId, PageMaster.Role).FirstOrDefault();
                        if (result != null)
                        {
                            roleId = result.InsertedId;
                            foreach (var item in objSave.Rights.Split(','))
                            {
                                string[] strRight = item.Split('|');
                                this.objDataContext.InsertOrUpdateRolePermissions(strRight[0].LongSafe(), roleId, strRight[1].LongSafe(), strRight[2].BoolSafe(), strRight[3].BoolSafe(), strRight[4].BoolSafe(), strRight[5].BoolSafe(), strRight[6].BoolSafe(), MySession.Current.UserId, PageMaster.Role);
                            }
                        }
                    }

                    scope.Complete();
                }

                return roleId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Determines whether [is role exists] [the specified long role identifier].
        /// </summary>
        /// <param name="lgRoleId">The long role identifier.</param>
        /// <param name="strRoleName">Name of the string role.</param>
        /// <returns><c>true</c> if [is role exists] [the specified long role identifier]; otherwise, <c>false</c>.</returns>
        public bool IsRoleExists(long lgRoleId, string strRoleName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (this.objDataContext.Roles.Where(x => x.Id != lgRoleId && x.RoleName == strRoleName && x.IsDeleted == false).Count() > 0)
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
        /// Delete RoleData.
        /// </summary>
        /// <param name="strRoleIdList">The string role identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>DeleteRole Result.</returns>
        public DeleteRoleResult DeleteRole(string strRoleIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            System.Data.Common.DbTransaction tran = null;
            try
            {
                this.objDataContext.Connection.Open();
                tran = this.objDataContext.Connection.BeginTransaction();
                this.objDataContext.Transaction = tran;
                DeleteRoleResult result = this.objDataContext.DeleteRole(strRoleIdList, lgDeletedBy, PageMaster.Role).FirstOrDefault();
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
        /// Searches the role.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;SearchRoleResult  &gt;.</returns>
        public List<SearchRoleResult> SearchRole(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.objDataContext.SearchRole(inRow, inPage, strSearch, strSort).ToList();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// GET the role permission by role identifier.
        /// </summary>
        /// <param name="lgRoleId">The long role identifier.</param>
        /// <returns>List&lt;GetPagePermissionResult  &gt;.</returns>
        public List<GetPagePermissionResult> GerRolePermissionByRoleId(long lgRoleId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                CommonDataContext objCommon = new CommonDataContext();
                return objCommon.GetPagePermission(0, 0, lgRoleId).ToList();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
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
    }
}
