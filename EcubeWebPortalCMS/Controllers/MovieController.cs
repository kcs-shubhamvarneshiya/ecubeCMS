// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="MovieController.cs" company="KCSPL">
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
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class MovieController.
    /// </summary>
    public class MovieController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i movie command.
        /// </summary>
        private readonly IMovieCommand objIMovieCommand = null;

        /// <summary>
        /// The object i movie theater command.
        /// </summary>
        private readonly IMovieTheatreCommand objIMovieTheatreCommand = null;

        /// <summary>
        /// The object i movie show command.
        /// </summary>
        private readonly IMovieShowCommand objIMovieShowCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieController" /> class.
        /// </summary>
        /// <param name="iMovieCommand">The i movie command.</param>
        public MovieController(IMovieCommand iMovieCommand)
        {
            IMovieTheatreCommand iMovieTheatreCommand = new MovieTheatreCommand();
            this.objIMovieTheatreCommand = iMovieTheatreCommand;
            IMovieShowCommand iMovieShowCommand = new MovieShowCommand();
            this.objIMovieShowCommand = iMovieShowCommand;
            this.objIMovieCommand = iMovieCommand;
        }

        /// <summary>
        /// Movies the view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult MovieView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateMovie));
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
        /// Movies this instance.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult Movie()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateMovie));

                MovieModel objMovieModel = new MovieModel();
                Random ran = new Random();
                objMovieModel.HdnSession = ran.Next().ToString();
                long lgMovieId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objMovieModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgMovieId = this.Request.QueryString.ToString().Decode().LongSafe();
                        objMovieModel = this.objIMovieCommand.GetMovieByMovieId(lgMovieId);
                        objMovieModel.HdnSession = ran.Next().ToString();
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                this.BindDropDownListForMovie(objMovieModel, true);

                objMovieModel.LstMovieTheatre = this.objIMovieTheatreCommand.GetAllMovieTheatreForDropDown();
                this.Session["ProfilePic"] = objMovieModel.UploadImage;
                objMovieModel.UploadImage = objMovieModel.UploadImage == null ? string.Empty : Convert.ToString(objMovieModel.UploadImage.ImageOrDefault());
                ////this.Session["MobileImage"] = objMovieModel.MobileImage;
                if (!string.IsNullOrEmpty(objMovieModel.MobileImage))
                {
                    objMovieModel.ShowMobileImage = string.Empty;
                    objMovieModel.ShowMobileImage = "../CMSUpload/Document/Movie/" + objMovieModel.Id + "/Original.jpg?" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                }

                this.Session["MobileImage"] = objMovieModel.MobileImage;
                objMovieModel.LstMovieshows = this.objIMovieShowCommand.GetAllMovieShowForDropDown();
                this.CreateDetails(objMovieModel.HdnSession, lgMovieId);
                return this.View(objMovieModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Movies the specified object movie.
        /// </summary>
        /// <param name="objMovie">The object movie.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult Movie(MovieModel objMovie)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateMovie));

                if (objMovie.Id == 0)
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

                if (objMovie.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                string movieShows = string.Empty;
                List<string> MovieShowForInActiveMovie = new List<string>();
                bool blExists = this.objIMovieCommand.IsMovieExists(objMovie.Id, objMovie.MovieName);
                //bool blExists = this.objIMovieCommand.IsMovieExists(objMovie.Id, objMovie.MovieName);
                if (objMovie.Id > 0 && !objMovie.IsActive)
                {
                    List<GetMovieShowForInActiveMovieResult> movieShowsForInActiveMovie = this.objIMovieCommand.GetMovieShowForInActiveMovie(Convert.ToInt32(objMovie.Id));
                    if (movieShowsForInActiveMovie != null && movieShowsForInActiveMovie.Count > 0)
                    {
                        foreach (var item in movieShowsForInActiveMovie)
                        {
                            if (item.MovieShow != null && item.MovieShow.Value != null)
                            {
                                MovieShowForInActiveMovie.Add(item.MovieShow.Value.ToString("dd/MM/yyyy hh:mm tt"));
                            }
                        }
                    }

                    if (MovieShowForInActiveMovie != null && MovieShowForInActiveMovie.Count > 0)
                    {
                        movieShows = "Following Show are currently running in theatre. Thus movie can not be In-Active.\n" + string.Join(",\n", MovieShowForInActiveMovie);
                    }
                }

                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Movie", MessageType.AlreadyExist);
                }
                else if (!string.IsNullOrEmpty(movieShows))
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = movieShows;
                }
                else
                {
                    string strErrorMsg = this.ValidateMovie(objMovie);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        List<MovieModel> objMovieTheatreShowList = (List<MovieModel>)this.Session["MovieTheatreShow" + objMovie.HdnSession];
                        if (this.Session["ProfilePic"] != null)
                        {
                            objMovie.UploadImage = this.Session["ProfilePic"].ToString();
                        }

                        if (this.Session["MobileImage"] != null)
                        {
                            objMovie.MobileImage = this.Session["MobileImage"].ToString();
                        }

                        if (objMovie.Id > 0)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Movie", MessageType.UpdateSuccess);
                        }

                        objMovie.Id = this.objIMovieCommand.SaveMovie(objMovie, objMovieTheatreShowList);
                        if (objMovie.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                            {
                                this.ViewData["Message"] = Functions.AlertMessage("Movie", MessageType.Success);
                            }

                            this.BindDropDownListForMovie(objMovie, false);
                            if (this.Session["MobileImagePath"] != null)
                            {
                                Functions.ReSizeBanner(this.Session["MobileImagePath"].ToString(), this.Session["MobileImage"].ToString(), Convert.ToString(objMovie.Id), "Movie");
                            }

                            objMovie.LstMovieTheatre = this.objIMovieTheatreCommand.GetAllMovieTheatreForDropDown();
                            objMovie.LstMovieshows = this.objIMovieShowCommand.GetAllMovieShowForDropDown();
                            return this.View(objMovie);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Movie", MessageType.Fail);
                        }
                    }
                }

                this.BindDropDownListForMovie(objMovie, true);
                return this.View(objMovie);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Movie", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                this.BindDropDownListForMovie(objMovie, true);
                return this.View(objMovie);
            }
        }

        /// <summary>
        /// Binds the movie grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <param name="isActive">The is Active.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindMovieGrid(string sidx, string sord, int page, int rows, string filters, string search, bool isActive)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchMovieResult> objMovieList = this.objIMovieCommand.SearchMovie(rows, page, search, sidx + " " + sord, isActive == null ? true : isActive);
                if (objMovieList != null && objMovieList.Count > 0)
                {
                    return this.FillGridMovie(page, rows, objMovieList);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the movie.
        /// </summary>
        /// <param name="strMovieId">The string movie identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteMovie(string strMovieId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strMovie = strMovieId.Split(',');
                strMovieId = string.Empty;

                foreach (var item in strMovie)
                {
                    strMovieId += item.Decode() + ",";
                }

                strMovieId = strMovieId.Substring(0, strMovieId.Length - 1);
                DeleteMovieResult result = this.objIMovieCommand.DeleteMovie(strMovieId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Movie", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Movie", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Movie", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Movie", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the movie.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetMovie()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objIMovieCommand.GetAllMovieForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Creates the details.
        /// </summary>
        /// <param name="strSessionId">The string session identifier.</param>
        /// <param name="lgMovieId">The long movie identifier.</param>
        public void CreateDetails(string strSessionId, long lgMovieId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                this.Session.Add("MovieTheatreShow" + strSessionId, this.objIMovieCommand.GetMovieTheatreShowByMovieId(lgMovieId));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        /// <summary>
        /// Movies the theatre show.
        /// </summary>
        /// <param name="objMovie">The object movie.</param>
        /// <returns>JSON  Result.</returns>
        [HttpPost]
        public JsonResult MovieTheatreShow(MovieModel objMovie)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = this.ValidateMovieTheatreShow(objMovie);

                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    return this.Json(strErrorMsg);
                }

                if (this.Session["MovieTheatreShow" + objMovie.HdnSession] == null)
                {
                    this.CreateDetails(objMovie.HdnSession, objMovie.Id);
                }

                List<MovieModel> objMovieTheatreShowList = (List<MovieModel>)this.Session["MovieTheatreShow" + objMovie.HdnSession];
                MovieModel objMovieModel = new MovieModel();
                if (objMovie.MovieTheatreShowId > 0)
                {
                    objMovieModel = objMovieTheatreShowList.Where(x => x.Id == objMovie.MovieTheatreShowId).FirstOrDefault();
                }

                if (objMovieModel == null)
                {
                    objMovieModel = new MovieModel();
                }

                if (objMovieModel.Id == 0)
                {
                    objMovieModel.Id = objMovieTheatreShowList.Count() + 1;
                }

                objMovieModel.MovieTheatreShowName = objMovie.MovieTheatreShowName;
                objMovieModel.ShowKeyDate = objMovie.StrShowKeyDate.DateSafe();
                objMovieModel.MovieTheatreId = objMovie.MovieTheatreId;
                objMovieModel.MovieShowId = objMovie.MovieShowId;
                objMovieModel.MemberAmount = objMovie.MemberAmount;
                objMovieModel.GuestAmount = objMovie.GuestAmount;
                objMovieModel.IsDeleted = false;

                bool exists = this.objIMovieCommand.IsMovieShowExists(Convert.ToInt64(objMovieModel.Id), Convert.ToInt32(objMovieModel.MovieShowId), objMovieModel.ShowKeyDate.Value, Convert.ToInt32(objMovieModel.MovieTheatreId));
                bool tempExists = false;
                if (objMovieTheatreShowList.Where(x => x.Id != objMovieModel.Id && x.ShowKeyDate.Value == objMovieModel.ShowKeyDate && x.MovieShowId == objMovieModel.MovieShowId && x.MovieTheatreId == objMovieModel.MovieTheatreId).Count() > 0)
                {
                    tempExists = true;
                }

                if (!exists && !tempExists)
                {
                    if (objMovie.MovieTheatreShowId == 0)
                    {
                        objMovieTheatreShowList.Add(objMovieModel);
                    }

                    this.Session.Add("MovieTheatreShow" + objMovie.HdnSession, objMovieTheatreShowList);
                    return this.Json("1111");
                }
                else
                {
                    return this.Json("Show is already exists!!");
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Binds the movie theatre show grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="hdnSession">The HDN session.</param>
        /// <param name="lgMovieId">The long movie identifier.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindMovieTheatreShowGrid(string sidx, string sord, int page, int rows, string filters, string hdnSession, long lgMovieId = 0)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (this.Session["MovieTheatreShow" + hdnSession] == null)
                {
                    this.CreateDetails(hdnSession, lgMovieId);
                }

                List<MovieModel> objMovieTheatreShowList = (List<MovieModel>)this.Session["MovieTheatreShow" + hdnSession];
                objMovieTheatreShowList = objMovieTheatreShowList.Where(x => x.IsDeleted == false).ToList();
                if (objMovieTheatreShowList != null && objMovieTheatreShowList.Count > 0)
                {
                    return this.FillGridMovieTheatreShow(page, rows, objMovieTheatreShowList);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the movie theatre show.
        /// </summary>
        /// <param name="strMovieTheatreShowId">The string movie theatre show identifier.</param>
        /// <param name="strSessionId">The string session identifier.</param>
        /// <param name="lgMovieId">The long movie identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteMovieTheatreShow(string strMovieTheatreShowId, string strSessionId, long lgMovieId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (this.Session["MovieTheatreShow" + strSessionId] == null)
                {
                    this.CreateDetails(strSessionId, lgMovieId);
                    return this.Json(string.Empty);
                }

                List<MovieModel> objMovieTheatreShowList = (List<MovieModel>)this.Session["MovieTheatreShow" + strSessionId];
                string[] strMovieTheatreShow = strMovieTheatreShowId.Split(',');
                foreach (var item in strMovieTheatreShow)
                {
                    var result = objMovieTheatreShowList.FirstOrDefault(x => x.Id == item.LongSafe());
                    if (result != null)
                    {
                        result.IsDeleted = true;
                    }
                }

                this.Session.Add("MovieTheatreShow" + strSessionId, objMovieTheatreShowList);
                return this.Json("Success");
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Removes the sessions.
        /// </summary>
        /// <param name="strSessionId">The string session identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult RemoveSessions(string strSessionId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                this.Session.Remove("MovieTheatreShow" + strSessionId);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return this.Json(string.Empty);
        }

        /// <summary>
        /// Uploads the profile pic.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult UploadImageData(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (this.Request.Files["files"] != null)
                {
                    string strDirPath = this.Request.MapPath("../Images/");
                    using (var binaryReader = new BinaryReader(this.Request.Files["files"].InputStream))
                    {
                        BanquetModel objBanquetModel = new BanquetModel();
                        var imagefile = binaryReader.ReadBytes(this.Request.Files["files"].ContentLength);
                        string savepath = strDirPath + "Movies/";
                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }

                        ////your image
                        string extension = Path.GetExtension(this.Request.Files["files"].FileName);
                        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".bmp" || extension == ".gif" || extension == ".png")
                        {
                            this.Request.Files["files"].SaveAs(savepath + this.Request.Files["files"].FileName);
                            this.Session["ProfilePic"] = this.Request.Files["files"].FileName;
                        }
                        else
                        {
                            return this.Json("UPloaded File not in Proper Format.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

            }

            return this.Json("1111");
        }

        /// <summary>
        /// Uploads the event banner image.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult UploadMobileImage(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            if (this.Request.Files["files"] != null)
            {
                if (this.Request.Files.Count > 0)
                {
                    var file = this.Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] bytes = new byte[20];
                        file.InputStream.Read(bytes, 0, 20);
                        if (!Functions.CheckFileExtension(file.FileName, Functions.AllowedFileExtensions))
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensions));
                        }
                        else
                        {
                            this.ViewData["Success"] = "1";
                            string fileName = Path.GetFileName(file.FileName).Replace(" ", string.Empty).Replace("_", string.Empty);
                            string strImagepath = this.Request.MapPath("../CMSUpload/Document/");
                            bool exists = Directory.Exists(strImagepath);
                            if (!exists)
                            {
                                Directory.CreateDirectory(strImagepath);
                            }

                            var path = Path.Combine(strImagepath, fileName);
                            Image newImage = Bitmap.FromStream(file.InputStream);
                            try
                            {
                                newImage.Save(path, ImageFormat.Jpeg);
                            }
                            catch (Exception ex)
                            {
                                information += " - " + ex.Message;
                                logger.Error(ex, information);
                            }

                            newImage.Dispose();
                            this.Session["MobileImagePath"] = strImagepath;
                            this.Session["MobileImage"] = this.Request.Files["files"].FileName;
                        }
                    }
                }
            }

            return this.Json("1111");
        }

        /// <summary>
        /// Fills the grid movie theatre show.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objMovieTheatreShowList">The object movie theatre show list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridMovieTheatreShow(int page, int rows, List<MovieModel> objMovieTheatreShowList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                int totalRecords = objMovieTheatreShowList != null ? objMovieTheatreShowList.Count : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objMovieTheatreShow in objMovieTheatreShowList
                            select new
                            {
                                Id = objMovieTheatreShow.Id.ToString(),
                                ShowKeyDate = Convert.ToDateTime(objMovieTheatreShow.ShowKeyDate.ToString()).ToShortDateString().Replace("-", "/"),
                                MovieTheatreId = objMovieTheatreShow.MovieTheatreId.ToString(),
                                MovieTheatre = this.objIMovieTheatreCommand.GetAllMovieTheatreForDropDown().Where(x => x.Value == objMovieTheatreShow.MovieTheatreId.ToString()).FirstOrDefault().Text,
                                MovieShowId = objMovieTheatreShow.MovieShowId.ToString(),
                                MovieShow = this.objIMovieShowCommand.GetAllMovieShowForDropDown().Where(x => x.Value == objMovieTheatreShow.MovieShowId.ToString()).FirstOrDefault().Text,
                                GuestAmount = objMovieTheatreShow.GuestAmount.ToString(),
                                MemberAmount = objMovieTheatreShow.MemberAmount.ToString()
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

        /// <summary>
        /// Validates the movie theatre show.
        /// </summary>
        /// <param name="objMovieTheatreShow">The object movie theatre show.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateMovieTheatreShow(MovieModel objMovieTheatreShow)
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
        /// Binds the drop down list for movie.
        /// </summary>
        /// <param name="objMovieModel">The object movie model.</param>
        /// <param name="blBindDropDownFromDb">IF set to <c>true</c> [BL bind drop down from database].</param>
        private void BindDropDownListForMovie(MovieModel objMovieModel, bool blBindDropDownFromDb)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (blBindDropDownFromDb)
                {
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        /// <summary>
        /// Fills the grid movie.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objMovieList">The object movie list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridMovie(int page, int rows, List<SearchMovieResult> objMovieList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objMovieList != null && objMovieList.Count > 0 ? (int)objMovieList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objMovie in objMovieList
                            select new
                            {
                                MovieName = objMovie.MovieName,
                                MovieCategory = objMovie.MovieCategory,
                                MovieRating = objMovie.MovieRating,
                                MovieLanguage = objMovie.MovieLanguage,
                                MovieContentType = objMovie.MovieContentType,
                                MovieType = objMovie.MovieType,
                                Id = objMovie.Id.ToString().Encode()
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

        /// <summary>
        /// Validates the movie.
        /// </summary>
        /// <param name="objMovie">The object movie.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateMovie(MovieModel objMovie)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objMovie.MovieName))
                {
                    strErrorMsg += Functions.AlertMessage("Movie Name", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return ex.Message.ToString();
            }
        }
    }
}