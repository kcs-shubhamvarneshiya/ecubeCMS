using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Models
{
    public interface IQuestionnaireCommand
    {
        List<SelectListItem> GetQuestionTypeForDropDown();
        bool IsQuestionnaireExists(long lgQuestionnaireId, string strQuestionnaireName);
        long SaveQuestionnaire(QuestionnaireModel objSave, List<QuestionModel> lstQuestion, ref string strPosition);
        QuestionnaireModel GetQuestionnaireByQuestionnaireId(long lgQuestionnaireId);
        long SaveQuestionnaireQuestion(QuestionModel objSave);
        List<SearchQuestionnaireResult> SearchQuestionnaire(int row, int page, string search, string sort,bool isActive);
        List<GetAllRecursionTypeResult> GetAllRecursionType();
        long UpdateRecursionDetailsByQuestionnaireId(QuestionnaireModel objSave);
    }
}