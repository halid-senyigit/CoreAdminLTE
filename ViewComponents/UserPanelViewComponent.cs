using CoreAdminLTE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAdminLTE.ViewComponents
{
    public class UserPanelViewComponent : ViewComponent
    {
        private readonly ModelContext db;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserPanelViewComponent(
            ModelContext db,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.db = db;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            string userName = httpContextAccessor.HttpContext.User.Identity.Name;
            ViewBag.userName = userName;
            return View();
        }

    }
}
