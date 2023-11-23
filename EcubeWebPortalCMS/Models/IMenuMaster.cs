using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcubeWebPortalCMS.Models
{
    public interface IMenuMaster
    {
        int MenuMobile(MenuMobileModel menuMobileModel);

        List<SubMenuMobileModel> SubMenuMobile(List<SubMenuMobileModel> subMenuMobileModel, int ParentMenuId);

        List<SearchMenuMobileResult> SearchMenuMobile(int inRow, int inPage, string strSearch, string strSort);

        MenuMobileModel GetMenuDetailsByMenuId(int MenuId);

        DeleteMobileMenuResult DeleteMobileMenu(string strMenuId, int userId);

        bool IsMenuExists(int MenuId, string MenuKey, string MenuDisplayName, int? SeqNo);

        string GetDuplicateField(int MenuId, string MenuKey, string MenuDisplayName, int? SeqNo);
    }
}
