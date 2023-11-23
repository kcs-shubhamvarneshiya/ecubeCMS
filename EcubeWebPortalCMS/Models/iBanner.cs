using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcubeWebPortalCMS.Models
{
    public interface iBanner
    {
        int SaveBanner(BannerModel bannerModel);

        List<SearchBannerResult> SearchBanner(int rows, int page, string search, string p);

        GetBannerByIdResult GetBannerById(int BannerId);

        DeleteBannerResult DeleteBanner(string strBannerId, int userId);

        bool IsBannerExists(int BannerId,string BannerTitle);
    }
}
