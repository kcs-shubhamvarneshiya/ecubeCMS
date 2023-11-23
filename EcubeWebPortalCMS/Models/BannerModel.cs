using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Models
{
    public class BannerModel
    {
        
        public int BannerId { get; set; }

        public string BannerTitle { get; set; }

        [AllowHtml]
        public string BannerDescription { get; set; }              

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string strFromDate { get; set; }

        public string strToDate { get; set; }
        
        public string Image1 { get; set; }
        
        public string Image2 { get; set; }

        public string Image3 { get; set; }
        
        public string Image4 { get; set; }

        public string Image5 { get; set; }

        public bool IsActive { get; set; }

        public string HdnSession { get; set; }
       
        public bool HdnIFrame { get; set; }
    }
}