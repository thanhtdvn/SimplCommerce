using Microsoft.AspNetCore.Mvc;
using SimplCommerce.Infrastructure.Web;

namespace SimplCommerce.Module.PaymentBankTransfer.Areas.PaymentBankTransfer.Components
{
    public class PaymentBankTransferLandingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(this.GetViewPath());
        }
    }
}
