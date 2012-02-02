using System.Web.Mvc;

namespace Jounce.Tests.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            return View();
        }      
    }
}
