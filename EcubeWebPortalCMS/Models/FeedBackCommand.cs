using EcubeWebPortalCMS.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EcubeWebPortalCMS.Models
{
    public class FeedBackCommand : IFeedBackCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        private readonly QuestionnaireDataContext objDataContext = new QuestionnaireDataContext();

        public List<GetFeedbackAnwserListResult> GetFeedbackAnwserList(long questionnaireId, string memberCode, DateTime? fromDate, DateTime? toDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<GetFeedbackAnwserListResult> feedbackAnswerList = this.objDataContext.GetFeedbackAnwserList(questionnaireId, memberCode, fromDate, toDate).ToList();
                return feedbackAnswerList;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

                return null;
            }
        }
    }
}