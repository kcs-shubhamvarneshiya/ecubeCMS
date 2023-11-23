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

    public class QuestionnaireCommand : IQuestionnaireCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        private readonly QuestionnaireDataContext objDataContext = new QuestionnaireDataContext();

        public List<SelectListItem> GetQuestionTypeForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();


            List<SelectListItem> objUserList = new List<SelectListItem>();
            try
            {
                objUserList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                List<GetAllFeedbackQuestionTypeResult> objUserResultList = this.objDataContext.GetAllFeedbackQuestionType().ToList();
                if (objUserResultList.Count > 0)
                {
                    foreach (var item in objUserResultList)
                    {
                        objUserList.Add(new SelectListItem { Text = item.QuestionTypeName, Value = item.Id.ToString() });
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

        public bool IsQuestionnaireExists(long questionnaireId, string questionnaireName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (this.objDataContext.Questionnaires.Where(x => x.Id != questionnaireId && x.QuestionnaireTitle == questionnaireName && x.IsDeleted == false).Count() > 0)
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

        public long SaveQuestionnaire(QuestionnaireModel objSave, List<QuestionModel> lstQuestion, ref string position)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();


            try
            {
                long QuestionnaireId = 0;
                if (objSave.Id > 0 && !string.IsNullOrEmpty(objSave.DeletedQuestionId))
                {
                    this.objDataContext.DeleteQuestions(objSave.DeletedQuestionId, MySession.Current.UserId, PageMaster.FeedbackForm);
                }

                QuestionnaireId = this.objDataContext.SaveQuestionnaire(objSave.Id, objSave.QuestionnaireTitle, objSave.Description, MySession.Current.UserId, objSave.IsActive).FirstOrDefault().QuestionnaireId;
                if (QuestionnaireId > 0)
                {
                    long QuestionId = 0;
                    foreach (QuestionModel item in lstQuestion)
                    {
                        if (item.QuestionId == 0)
                        {
                            QuestionId = this.objDataContext.SaveQuestion(item.QuestionId, QuestionnaireId, item.Position, item.QuestionTypeId, item.QuestionTitle, item.ShortName, item.IsActive, item.Required, item.IsDisplayInSummary, item.IsDisplayInDetail, item.MaxLength, item.Hint, item.EscalationRegex, item.OptionDisplayType, item.IsTitleBold, item.IsTitleItalic, item.IsTitleUnderline, item.TitleTextColor, item.TableGroupName, item.Margin, item.FontSize, 0, 0, 0, 0, item.ImagePath, item.Imageheight, item.Imagewidth, item.Imagealign, MySession.Current.UserId, item.IsDelete).FirstOrDefault().QuestionId;
                            if (item.LstOption != null)
                            {
                                foreach (OptionsModel opt in item.LstOption)
                                {
                                    this.objDataContext.SaveOption(0, QuestionId, opt.Position, opt.Name, opt.Value, opt.DefaultValue, opt.Point, MySession.Current.UserId);
                                }
                            }
                        }
                        else
                        {
                            QuestionId = item.QuestionId;
                        }

                        if (string.IsNullOrEmpty(position))
                        {
                            position = QuestionId.ToString();
                        }
                        else
                        {
                            position += "," + QuestionId.ToString();
                        }
                    }
                }

                objSave.Id = QuestionnaireId;

                return objSave.Id;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        public QuestionnaireModel GetQuestionnaireByQuestionnaireId(long questionnaireId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            var objQuestionnaireModel = new QuestionnaireModel();
            try
            {
                GetQuestionnaireByIdResult item = this.objDataContext.GetQuestionnaireById(questionnaireId).ToList().FirstOrDefault();
                if (item != null)
                {
                    objQuestionnaireModel.Id = item.Id;
                    objQuestionnaireModel.QuestionnaireTitle = item.QuestionnaireTitle;
                    objQuestionnaireModel.Description = item.Description;
                    objQuestionnaireModel.LstQuestionnaireQuestion = this.objDataContext.GetQuestionsByQuestionnaireId(questionnaireId).OrderBy(x => x.Position).ToList();
                    objQuestionnaireModel.LstQuestionnaireOptions = this.objDataContext.GetOptionsByQuestionnaireId(questionnaireId).ToList();
                    objQuestionnaireModel.IsActive = item.IsActive;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

            }

            return objQuestionnaireModel;
        }

        public long SaveQuestionnaireQuestion(QuestionModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();


            try
            {
                objSave.QuestionId = this.objDataContext.SaveQuestion(objSave.QuestionId, objSave.QuestionnaireId, objSave.Position, objSave.QuestionTypeId, objSave.QuestionTitle, objSave.ShortName, objSave.IsActive, objSave.Required, objSave.IsDisplayInSummary, objSave.IsDisplayInDetail, objSave.MaxLength, objSave.Hint, objSave.EscalationRegex, objSave.OptionDisplayType, objSave.IsTitleBold, objSave.IsTitleItalic, objSave.IsTitleUnderline, objSave.TitleTextColor, objSave.TableGroupName, objSave.Margin, objSave.FontSize, 0, 0, 0, 0, objSave.ImagePath, objSave.Imageheight, objSave.Imagewidth, objSave.Imagealign, MySession.Current.UserId, objSave.IsDelete).FirstOrDefault().QuestionId;
                if ((objSave.QuestionTypeId == 1 || objSave.QuestionTypeId == 5 || objSave.QuestionTypeId == 6 || objSave.QuestionTypeId == 18 || objSave.QuestionTypeId == 21) && objSave.LstOption != null)
                {
                    foreach (OptionsModel opt in objSave.LstOption)
                    {
                        this.objDataContext.SaveOption(opt.Id, objSave.QuestionId, opt.Position, opt.Name, opt.Value, opt.DefaultValue, opt.Point, MySession.Current.UserId);
                    }
                }

                return objSave.QuestionId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        public List<SearchQuestionnaireResult> SearchQuestionnaire(int row, int page, string search, string sort, bool isActive)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();


            try
            {
                List<SearchQuestionnaireResult> lstSearchQuestionnaire = this.objDataContext.SearchQuestionnaire(row, page, search, sort, isActive).ToList();
                return lstSearchQuestionnaire;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        public List<GetAllRecursionTypeResult> GetAllRecursionType()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();


            try
            {
                List<GetAllRecursionTypeResult> recursionTypeList = this.objDataContext.GetAllRecursionType().ToList();
                return recursionTypeList;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        public long UpdateRecursionDetailsByQuestionnaireId(QuestionnaireModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();


            try
            {
                objSave.Id = this.objDataContext.UpdateRecursionDetailsByQuestionnaireId(objSave.Id, objSave.IsRecursionCreated, objSave.RecursionTypeId, objSave.RecursionFrom, objSave.RecursionTo, objSave.RecursionSMS, objSave.RecursionEmail, MySession.Current.UserId).FirstOrDefault().QuestionnaireId;
                return objSave.Id;
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