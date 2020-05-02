using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MyCashFlow.Client.Serialization.Responses
{
    public class StockChangeData
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("changed_at")]
        public DateTimeOffset ChangedAt { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("source_type")]
        public string SourceType { get; set; }

        [JsonProperty("source_id")]
        public int? SourceId { get; set; }

        [JsonProperty("stock_item_id")]
        public int StockItemId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("quantity_change")]
        public int QuantityChange { get; set; }
    }

    public class Meta
    {

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("page_count")]
        public int PageCount { get; set; }

        [JsonProperty("item_count")]
        public int ItemCount { get; set; }
    }

    public class StockChangeResponse
    {

        [JsonProperty("data")]
        public IList<StockChangeData> Data { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }
}
