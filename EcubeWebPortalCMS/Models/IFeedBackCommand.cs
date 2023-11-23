using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcubeWebPortalCMS.Models
{
    public interface IFeedBackCommand
    {
        List<GetFeedbackAnwserListResult> GetFeedbackAnwserList(long questionnaireId, string memberCode, DateTime? fromDate, DateTime? toDate);
    }
}