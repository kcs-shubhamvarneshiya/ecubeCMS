using EcubeWebPortalCMS.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace EcubeWebPortalCMS.Models
{
    public class BannerCommand : iBanner
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ECubeCMSConnectionString"].ConnectionString);

        private BannerDataDataContext objDataContext = new BannerDataDataContext();

        public int SaveBanner(BannerModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int bannerId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new BannerDataDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateBanner(objSave.BannerId, objSave.BannerTitle, objSave.BannerDescription, objSave.FromDate, objSave.ToDate, objSave.Image1, objSave.Image2, objSave.Image3, objSave.Image4, objSave.Image5, objSave.IsActive, MySession.Current.UserId, PageMaster.Post).FirstOrDefault();
                        if (result != null)
                        {
                            bannerId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return bannerId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        public List<SearchBannerResult> SearchBanner(int rows, int page, string search, string p)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new BannerDataDataContext())
                {
                    List<SearchBannerResult> objSearchBannerList = this.objDataContext.SearchBanner(rows, page, search, p).ToList();
                    return objSearchBannerList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        public GetBannerByIdResult GetBannerById(int BannerId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();


            try
            {
                using (this.objDataContext = new BannerDataDataContext())
                {
                    GetBannerByIdResult getBannerByIdResult = this.objDataContext.GetBannerById(BannerId).FirstOrDefault();
                    return getBannerByIdResult;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        public DeleteBannerResult DeleteBanner(string strBannerId, int userId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            DeleteBannerResult result = new DeleteBannerResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new BannerDataDataContext())
                    {
                        result = this.objDataContext.DeleteBanner(strBannerId, userId, PageMaster.Post).ToList().FirstOrDefault();
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

        public bool IsBannerExists(int BannerId, string BannerTitle)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new BannerDataDataContext())
                {
                    
                    if (this.objDataContext.Banners.Where(x => x.BannerId != BannerId && x.BannerTitle == BannerTitle  && x.IsDeleted == false).Any())
                    {
                        return true; // A matching banner exists
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
    }
}