using System.Web.Mvc;
using CrmDemoApp.Infrastructure;

namespace CrmDemoApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult ViewContacts()
        {
            var xrmServiceContext = new XrmServiceConnector().CreateXrmServiceConnector();
            var contacts = xrmServiceContext.ContactSet;
            return View(contacts);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}