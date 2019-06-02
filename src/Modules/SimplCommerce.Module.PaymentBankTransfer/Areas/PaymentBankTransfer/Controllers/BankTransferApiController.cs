using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.PaymentBankTransfer.Models;
using SimplCommerce.Module.Payments.Models;

namespace SimplCommerce.Module.PaymentBankTransfer.Areas.PaymentBankTransfer.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("PaymentBankTransfer")]
    [Route("api/banktransfer")]
    public class BankTransferApiController : Controller
    {
        private readonly IRepositoryWithTypedId<PaymentProvider, string> _paymentProviderRepository;

        public BankTransferApiController(IRepositoryWithTypedId<PaymentProvider, string> paymentProviderRepository)
        {
            _paymentProviderRepository = paymentProviderRepository;
        }

        [HttpGet("config")]
        public async Task<IActionResult> Config()
        {
            var provider = await _paymentProviderRepository.Query().FirstOrDefaultAsync(x => x.Id == PaymentProviderHelper.BankTransferProviderId);
            if (string.IsNullOrEmpty(provider.AdditionalSettings))
            {
                return Ok(new BankTransferSetting());
            }

            var model = JsonConvert.DeserializeObject<BankTransferSetting>(provider.AdditionalSettings);
            return Ok(model);
        }

        [HttpPut("config")]
        public async Task<IActionResult> Config([FromBody] BankTransferSetting model)
        {
            if (ModelState.IsValid)
            {
                var provider = await _paymentProviderRepository.Query().FirstOrDefaultAsync(x => x.Id == PaymentProviderHelper.BankTransferProviderId);
                provider.AdditionalSettings = JsonConvert.SerializeObject(model);
                await _paymentProviderRepository.SaveChangesAsync();
                return Accepted();
            }

            return BadRequest(ModelState);
        }
    }
}
