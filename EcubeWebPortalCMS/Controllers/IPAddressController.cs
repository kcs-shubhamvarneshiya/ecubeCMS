// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="IPAddressController.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace EcubeWebPortalCMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class IPAddressController.
    /// </summary>
    public class IPAddressController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Zero-based index of the IP  address.
        /// </summary>
        private readonly IIPAddressCommand objIIPAddressCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="IPAddressController"/> class.
        /// </summary>
        /// <param name="iIPAddressCommand">The IP  address command.</param>
        public IPAddressController(IIPAddressCommand iIPAddressCommand)
        {
            this.objIIPAddressCommand = iIPAddressCommand;
        }

        /// <summary>
        /// IP  the address view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult IPAddressView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.IPAddress);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (!objPermission.View_Right)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = objPermission.Add_Right;
                this.ViewData["blEditRights"] = objPermission.Edit_Right;
                this.ViewData["blDeleteRights"] = objPermission.Delete_Right;
                return this.View();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// IP  the address.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult IPAddress()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.IPAddress);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                long lgIPAddressId = 0;
                IPAddressModel objIPAddress = new IPAddressModel();
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objIPAddress.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgIPAddressId = Request.QueryString.ToString().Decode().LongSafe();
                        objIPAddress = this.objIIPAddressCommand.GetIPAddressByIPAddressId(lgIPAddressId);
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objIPAddress);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// IP  the address.
        /// </summary>
        /// <param name="objIPAddress">The object IP  address.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult IPAddress(IPAddressModel objIPAddress)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.IPAddress);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objIPAddress.Id == 0)
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (!objPermission.Edit_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objIPAddress.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIIPAddressCommand.IsIPAddressExists(objIPAddress.Id, objIPAddress.IPAddressName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("IP Address", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateIPAddress(objIPAddress);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objIPAddress.Id = this.objIIPAddressCommand.SaveIPAddress(objIPAddress);
                        if (objIPAddress.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("IP Address", MessageType.Success);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("IP Address", MessageType.Fail);
                        }
                    }
                }

                return this.View(objIPAddress);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("IP Address", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objIPAddress);
            }
        }

        /// <summary>
        /// Binds the IP  address grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">PARAMETER SORD.</param>
        /// <param name="page">PARAMETER page.</param>
        /// <param name="rows">PARAMETER rows.</param>
        /// <param name="filters">PARAMETER filters.</param>
        /// <param name="search">PARAMETER search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindIPAddressGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchIPAddressResult> objIPAddressList = this.objIIPAddressCommand.SearchIPAddress(rows, page, search, sidx + " " + sord);
                return this.FillGridIPAddress(page, rows, objIPAddressList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the IP  address.
        /// </summary>
        /// <param name="strIPAddressId">The string IP  address identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteIPAddress(string strIPAddressId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strIPAddress = strIPAddressId.Split(',');
                strIPAddressId = string.Empty;
                foreach (var item in strIPAddress)
                {
                    strIPAddressId += item.Decode() + ",";
                }

                strIPAddressId = strIPAddressId.Substring(0, strIPAddressId.Length - 1);
                DeleteIPAddressResult result = this.objIIPAddressCommand.DeleteIPAddress(strIPAddressId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("IP Address", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("IP Address", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("IP Address", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("IP Address", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the IP  address.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetIPAddress()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objIIPAddressCommand.GetAllIPAddressForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validates the IP  address.
        /// </summary>
        /// <param name="objIPAddress">The object IP  address.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateIPAddress(IPAddressModel objIPAddress)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objIPAddress.IPAddressName))
                {
                    strErrorMsg += Functions.AlertMessage("IP Address", MessageType.InputRequired);
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return string.Empty;
            }
        }

        /// <summary>
        /// Fills the grid IP  address.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objIPAddressList">The object IP  address list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridIPAddress(int page, int rows, List<SearchIPAddressResult> objIPAddressList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objIPAddressList != null && objIPAddressList.Count > 0 ? objIPAddressList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objIPAddress in objIPAddressList
                            select new
                            {
                                IPAddressName = objIPAddress.IPAddressName,
                                Description = objIPAddress.Description,
                                IsActive = objIPAddress.IsActive.ToString(),
                                Id = objIPAddress.Id.ToString().Encode()
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
