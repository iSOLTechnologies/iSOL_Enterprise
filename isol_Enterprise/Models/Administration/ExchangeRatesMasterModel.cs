namespace iSOL_Enterprise.Models.Administration
{
    public class ExchangeRatesMasterModel
    {

        public int sno { get; set; }
        public DateTime? RateDate { get; set; }
        public string? Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
