using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using XrmUserStore;

namespace CrmDemoApp.Controllers
{
    public class HomeController : Controller
    {
        private XrmServiceConnection _xrmServiceConnection;


        public XrmServiceConnection XrmServiceConnection 
        {
            get
            {
                return _xrmServiceConnection ?? HttpContext.GetOwinContext().Get<XrmServiceConnection>();
            }
            private set
            {
                _xrmServiceConnection = value;
            }
        }


        [Authorize]
        public ActionResult ViewContacts()
        {
            var contacts = XrmServiceConnection.XrmServiceContext.ContactSet.ToList();
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