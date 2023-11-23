using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EcubeWebPortalCMS.Models
{
    public class MenuMobileModel
    {
        public int MenuId { get; set; }
        public int? ParentMenuId { get; set; }
        public int? SeqNo { get; set; }
        public string MenuKey { get; set; }
        public string MenuDisplayName { get; set; }
        public string MenuIcon { get; set; }
        public int UserId { get; set; }    
        public bool isDeleted { get; set; }    
        public string IconImages { get; set; }
        public string HdnSession { get; set; }
        public bool HdnIFrame { get; set; }
        public List<SubMenuMobileModel> MenuMobileModelList { get; set; }

    }
    public class SubMenuMobileModel
    {
        public int SubMenuId { get; set; }
        public int? SubParentMenuId { get; set; }
        public int? SubSeqNo { get; set; }
        public string SubMenuKey { get; set; }
        public string SubMenuDisplayName { get; set; }
        public string SubMenuIcon { get; set; }       
        public bool SubisDeleted { get; set; }
        public string SubIconImages { get; set; }       
    }
}