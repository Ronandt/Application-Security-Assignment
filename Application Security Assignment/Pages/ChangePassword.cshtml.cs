using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Security_Assignment.Pages
{
    public class ChangePasswordModel : PageModel
    {
        [BindProperty]
        public string Password { get; set; }
        public void OnGet()
        {

        }
    }
}
