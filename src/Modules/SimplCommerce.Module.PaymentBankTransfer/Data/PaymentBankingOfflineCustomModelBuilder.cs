using Microsoft.EntityFrameworkCore;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Payments.Models;
using SimplCommerce.Module.PaymentBankTransfer.Models;

namespace SimplCommerce.Module.PaymentBankTransfer.Data
{
    public class PaymentBankTransferCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentProvider>().HasData(
                new PaymentProvider(PaymentProviderHelper.BankTransferProviderId) { Name = PaymentProviderHelper.BankTransferProviderName, LandingViewComponentName = "BankTransferLanding", ConfigureUrl = "payments-banktransfer-config", IsEnabled = true, AdditionalSettings = null }
            );
        }
    }
}
