using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RBN_FE.Pages.LogIn_Out
{
    public class PaymentQRModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("/LogIn&Out/SubscriptionPackageConfirm");
        }
    }
}
