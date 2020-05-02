using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyCashFlow.Client.Serialization.Responses
{
    public class ProductItemResponse
    {
        [JsonProperty("data")]
        public ProductItem Data { get; set; }

    }
}
