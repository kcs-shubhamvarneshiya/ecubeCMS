// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="BanquetBookingRequestController.cs" company="string.Empty">
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
    /// Class BanquetBookingRequestController.
    /// </summary>
    public class BanquetBookingRequestController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i banquet booking request command.
        /// </summary>
        private readonly IBanquetBookingRequestCommand objIBanquetBookingRequestCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Initializes a new instance of the <see cref="BanquetBookingRequestController" /> class.
        /// </summary>
        /// <param name="iBanquetBookingRequestCommand">The i banquet booking request command.</param>
        public BanquetBookingRequestController(IBanquetBookingRequestCommand iBanquetBookingRequestCommand)
        {
            this.objIBanquetBookingRequestCommand = iBanquetBookingRequestCommand;
        }

        /// <summary>
        /// Banquets the booking request view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult BanquetBookingRequestView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetHallRequest));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = getPageRights.Add;
                this.ViewData["blEditRights"] = getPageRights.Edit;
                this.ViewData["blDeleteRights"] = getPageRights.Delete;
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
        /// Banquets the booking request.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult BanquetBookingRequest()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetHallRequest));

                BanquetBookingRequestModel objBanquetBookingRequestModel = new BanquetBookingRequestModel();
                long lgBanquetBookingRequestId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objBanquetBookingRequestModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(Request.QueryString.ToString()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgBanquetBookingRequestId = Request.QueryString.ToString().LongSafe(); // .Decode().LongSafe();
                        objBanquetBookingRequestModel = this.objIBanquetBookingRequestCommand.GetBanquetBookingRequestByBanquetBookingRequestId(lgBanquetBookingRequestId);
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objBanquetBookingRequestModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Banquets the booking request.
        /// </summary>
        /// <param name="objBanquetBookingRequest">The object banquet booking request.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult BanquetBookingRequest(BanquetBookingRequestModel objBanquetBookingRequest)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetHallRequest));

                if (objBanquetBookingRequest.Id == 0)
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (getPageRights.Edit == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objBanquetBookingRequest.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                // bool blExists = objIBanquetBookingRequestCommand.IsBanquetBookingRequestExists(objBanquetBookingRequest.Id, objBanquetBookingRequest.Name);
                // if (blExists)
                // {
                // this.ViewData["Success"] = "0";
                // this.ViewData["Message"] = Functions.AlertMessage("Banquet Booking Request", MessageType.AlreadyExist);
                // }
                // else
                // {
                string strErrorMsg = this.ValidateBanquetBookingRequest(objBanquetBookingRequest);
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = strErrorMsg;
                }
                else
                {
                    objBanquetBookingRequest.Id = this.objIBanquetBookingRequestCommand.SaveBanquetBookingRequest(objBanquetBookingRequest);
                    if (objBanquetBookingRequest.Id > 0)
                    {
                        this.ViewData["Success"] = "1";
                        this.ViewData["Message"] = Functions.AlertMessage("Banquet Booking Request Status", MessageType.UpdateSuccess);
                        return this.View(objBanquetBookingRequest);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Banquet Booking Request Status", MessageType.Fail);
                    }
                }
                //// }
                return this.View(objBanquetBookingRequest);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Banquet Booking Request", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objBanquetBookingRequest);
            }
        }

        /// <summary>
        /// Binds the banquet booking request grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <param name="sortedby">The sorted by.</param>
        /// <param name="banquetType">Type of the banquet.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindBanquetBookingRequestGrid(string sidx, string sord, int page, int rows, string filters, string search, int sortedby, string banquetType)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                long lgBanquetType = 0;
                if (banquetType != string.Empty)
                {
                    lgBanquetType = Convert.ToInt64(banquetType);
                }
                else
                {
                    lgBanquetType = 0;
                }

                List<MS_SearchBanquetBookingRequestResult> objBanquetBookingRequestList = this.objIBanquetBookingRequestCommand.SearchBanquetBookingRequest(rows, page, search, sidx + " " + sord, sortedby, Convert.ToInt64(lgBanquetType));
                ////if (objBanquetBookingRequestList != null && objBanquetBookingRequestList.Count > 0)
                ////{
                return this.FillGridBanquetBookingRequest(page, rows, objBanquetBookingRequestList);
                ////}
                ////else
                ////{
                ////    return this.Json(string.Empty); 
                ////}
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the banquet booking request.
        /// </summary>
        /// <param name="strBanquetBookingRequestId">The string banquet booking request identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteBanquetBookingRequest(string strBanquetBookingRequestId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                //string[] strBanquetBookingRequest = strBanquetBookingRequestId.Split(',');
                //strBanquetBookingRequestId = string.Empty;
                //foreach (var item in strBanquetBookingRequest)
                //{
                //    strBanquetBookingRequestId += item.Decode() + ",";
                //}

                //strBanquetBookingRequestId = strBanquetBookingRequestId.Substring(0, strBanquetBookingRequestId.Length - 1);
                MS_DeleteBanquetBookingRequestResult result = this.objIBanquetBookingRequestCommand.DeleteBanquetBookingRequest(strBanquetBookingRequestId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Banquet Booking Request", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Banquet Booking Request", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Banquet Booking Request", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Banquet Booking Request", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the banquet booking request.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetBanquetBookingRequest()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objIBanquetBookingRequestCommand.GetAllBanquetBookingRequestForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the type of the banquet.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetBanquetType()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                BanquetBookingRequestCommand objBanquetType = new BanquetBookingRequestCommand();
                return this.Json(objBanquetType.GetBanquetType().ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validates the banquet booking request.
        /// </summary>
        /// <param name="objBanquetBookingRequest">The object banquet booking request.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateBanquetBookingRequest(BanquetBookingRequestModel objBanquetBookingRequest)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                return strErrorMsg;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// Fills the grid banquet booking request.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objBanquetBookingRequestList">The object banquet booking request list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridBanquetBookingRequest(int page, int rows, List<MS_SearchBanquetBookingRequestResult> objBanquetBookingRequestList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objBanquetBookingRequestList != null && objBanquetBookingRequestList.Count > 0 ? (int)objBanquetBookingRequestList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objBanquetBookingRequest in objBanquetBookingRequestList
                            select new
                            {
                                Name = objBanquetBookingRequest.Name,
                                MemberId = objBanquetBookingRequest.MemberId != null ? objBanquetBookingRequest.MemberId.Value.ToString() : string.Empty,
                                BookingType = objBanquetBookingRequest.BookingType != null ? objBanquetBookingRequest.BookingType.Value.ToString() : string.Empty,
                                InquiryFor = objBanquetBookingRequest.InquiryFor != null ? objBanquetBookingRequest.InquiryFor.Value.ToString() : string.Empty,
                                EMail = objBanquetBookingRequest.EMail,
                                Address = objBanquetBookingRequest.Address,
                                City = objBanquetBookingRequest.City,
                                MobileNo = objBanquetBookingRequest.MobileNo,
                                Occasion = objBanquetBookingRequest.Occasion,
                                FromDate = objBanquetBookingRequest.FromDate != null ? objBanquetBookingRequest.FromDate.Value.ToString(Functions.DateFormat) : string.Empty,
                                ToDate = objBanquetBookingRequest.ToDate != null ? objBanquetBookingRequest.ToDate.Value.ToString(Functions.DateFormat) : string.Empty,
                                BanquetType = objBanquetBookingRequest.BanquetType,
                                Remarks = objBanquetBookingRequest.Remarks,
                                StatusId = objBanquetBookingRequest.StatusId,
                                Status = objBanquetBookingRequest.Status,
                                AdminRemarks = objBanquetBookingRequest.AdminRemarks,
                                Id = objBanquetBookingRequest.Id.ToString() // .Encode()
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
