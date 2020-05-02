using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MyCashFlow.Client.Serialization.Responses
{
    public class ProductVariation
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("sort")]
        public int Sort { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("purchase_price")]
        public double? PurchasePrice { get; set; }

        [JsonProperty("weight")]
        public double? Weight { get; set; }
    }
}