using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Core.Extensions;
using SimplCommerce.Module.Orders.Services;
using SimplCommerce.Module.PaymentBankTransfer.Models;
using SimplCommerce.Module.Payments.Models;
using SimplCommerce.Module.ShoppingCart.Areas.ShoppingCart.ViewModels;
using SimplCommerce.Module.ShoppingCart.Services;

namespace SimplCommerce.Module.PaymentBankTransfer.Areas.PaymentBankTransfer.Controllers
{
    [Authorize]
    [Area("PaymentBankTransfer")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BankTransferController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IWorkContext _workContext;
        private readonly ICartService _cartService;
        private readonly IRepositoryWithTypedId<PaymentProvider, string> _paymentProviderRepository;
        private Lazy<BankTransferSetting> _setting;

        public BankTransferController(
            ICartService cartService,
            IOrderService orderService,
            IRepositoryWithTypedId<PaymentProvider, string> paymentProviderRepository,
            IWorkContext workContext)
        {
            _paymentProviderRepository = paymentProviderRepository;
            _cartService = cartService;
            _orderService = orderService;
            _workContext = workContext;
            _setting = new Lazy<BankTransferSetting>(GetSetting());
        }

        [HttpPost]
        public async Task<IActionResult> BankTransferCheckout()
        {
            var currentUser = await _workContext.GetCurrentUser();
            var cart = await _cartService.GetActiveCartDetails(currentUser.Id);
            if(cart == null)
            {
                return NotFound();
            }

            if (!ValidatePayment(cart))
            {
                TempData["Error"] = "Payment Method is not eligible for this order.";
                return Redirect("~/checkout/payment");
            }

            var calculatedFee = CalculateFee(cart);           
            var orderCreateResult = await _orderService.CreateOrder(cart.Id, "BankTransfer", calculatedFee);

            if (!orderCreateResult.Success)
            {
                TempData["Error"] = orderCreateResult.Error;
                return Redirect("~/checkout/payment");
            }

            return Redirect($"~/checkout/success?orderId={orderCreateResult.Value.Id}");
        }

        private BankTransferSetting GetSetting()
        {
            var provider = _paymentProviderRepository.Query().FirstOrDefault(x => x.Id == PaymentProviderHelper.BankTransferProviderId);
            if (string.IsNullOrEmpty(provider.AdditionalSettings))
            {
                return new BankTransferSetting();
            }

            var bankTransferSetting = JsonConvert.DeserializeObject<BankTransferSetting>(provider.AdditionalSettings);
            return bankTransferSetting;
        }

        private bool ValidatePayment(CartVm cart)
        {
            if (_setting.Value.MinOrderValue.HasValue && _setting.Value.MinOrderValue.Value > cart.OrderTotal)
            {
                return false;
            }

            if (_setting.Value.MaxOrderValue.HasValue && _setting.Value.MaxOrderValue.Value < cart.OrderTotal)
            {
                return false;
            }

            return true;
        }

        private decimal CalculateFee(CartVm cart)
        {
            var percent = _setting.Value.PaymentFee;
            return (cart.OrderTotal / 100) * percent;
        }
    }
}
