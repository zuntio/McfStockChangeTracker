using CsvHelper.Configuration;
using McfStockChangeTracker.Models;

namespace McfStockChangeTracker.CsvHelperClassMaps
{
    public class StockChangeDtoClassMap : ClassMap<StockChangeDto>
    {
        public StockChangeDtoClassMap()
        {
            int i = 0;
            Map(x => x.Product).Index(i++).Name("Product");
            Map(x => x.Variation).Index(i++).Name("Variation");
            Map(x => x.StockChangeType).Index(i++).Name("StockChangeType");
            Map(x => x.TimeStamp).Index(i++).Name("Timestamp");
            Map(x => x.User).Index(i++).Name("User");
            Map(x => x.Differential).Index(i++).Name("Differential");
            Map(x => x.TargetAmount).Index(i++).Name("TargetAmount");
        }
    }
}
