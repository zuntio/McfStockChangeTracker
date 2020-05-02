using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MyCashFlow.Client.Serialization.Responses
{
    public class ProductItem
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("supplier_code")]
        public string SupplierCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("information")]
        public string Information { get; set; }

        [JsonProperty("keywords")]
        public string Keywords { get; set; }

        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("purchase_price")]
        public double? PurchasePrice { get; set; }

        [JsonProperty("vat_rate")]
        public int? VatRate { get; set; }

        [JsonProperty("weight")]
        public double? Weight { get; set; }

        [JsonProperty("warranty")]
        public int? Warranty { get; set; }

        [JsonProperty("brand_id")]
        public int? BrandId { get; set; }

        [JsonProperty("supplier_id")]
        public int? SupplierId { get; set; }

        [JsonProperty("available_from")]
        public string AvailableFrom { get; set; }

        [JsonProperty("available_to")]
        public string AvailableTo { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("variations")]
        public IList<ProductVariation> Variations { get; set; }
    }

}