namespace SimplCommerce.Module.PaymentBankTransfer.Models
{
    public class BankTransferSetting
    {
        public decimal? MinOrderValue { get; set; }

        public decimal? MaxOrderValue { get; set; }

        public decimal PaymentFee { get; set; }
    }
}
