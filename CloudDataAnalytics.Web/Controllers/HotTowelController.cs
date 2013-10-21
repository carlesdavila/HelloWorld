using System.Web.Mvc;

namespace CloudDataAnalytics.Web.Controllers
{
    public class HotTowelController : Controller
    {
        //
        // GET: /HotTowel/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
