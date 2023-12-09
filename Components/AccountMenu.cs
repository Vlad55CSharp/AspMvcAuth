using Microsoft.AspNetCore.Mvc;
namespace AspMvcAuth.Components
{
    public class AccountMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(HttpContext.User);
        }
    }
}
