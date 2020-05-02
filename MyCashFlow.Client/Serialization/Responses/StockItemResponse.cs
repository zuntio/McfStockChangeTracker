using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MyCashFlow.Client.Serialization.Responses
{
    public class StockItemData
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("balance_limit")]
        public object BalanceLimit { get; set; }

        [JsonProperty("backorder_enabled")]
        public bool BackorderEnabled { get; set; }

        [JsonProperty("backorder_estimate")]
        public string BackorderEstimate { get; set; }

        [JsonProperty("balance")]
        public int Balance { get; set; }

        [JsonProperty("reserved")]
        public int Reserved { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("variation_id")]
        public int? VariationId { get; set; }

        [JsonProperty("balance_alert")]
        public object BalanceAlert { get; set; }
    }

    public class StockItemResponse
    {

        [JsonProperty("data")]
        public StockItemData Data { get; set; }
    }
}
