namespace McfStockChangeTracker.Models
{
    public class StockChangeDto
    {
        public string StockChangeType { get; set; }
        public string User { get; set; }
        public string Product { get; set; }
        public string Variation { get; set; }
        public string TimeStamp { get; set; }
        public int Differential { get; set; }
        public int TargetAmount { get; set; }
    }
}
