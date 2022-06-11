using System.Web;
using System.Web.Mvc;

namespace Patel_DBP_A7_Project3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
