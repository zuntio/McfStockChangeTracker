using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MyCashFlow.Client.Exceptions;
using MyCashFlow.Client.Serialization.Responses;
using Newtonsoft.Json;

namespace MyCashFlow.Client
{
    public class McfClient
    {
        public string BaseUrl { get; }
        private readonly HttpClient _client;

        private readonly string _baseUrl = "https://{0}.mycashflow.fi/api/v1";
        private readonly string _userName;
        private readonly string _apiKey;

        public McfClient(string storename, string username, string apiKey)
        {
            _baseUrl = string.Format(_baseUrl, storename);
            _client = new HttpClient();
            _userName = username;
            _apiKey = apiKey;
        }

        public async Task<StockItemResponse> GetStockItem(string productCode)
        {
            var uriBuilder = new StringBuilder();
            uriBuilder.Append(_baseUrl);
            uriBuilder.Append("/stock/");
            if (string.IsNullOrWhiteSpace(productCode))
            {
                throw new Exception("No valid ProductCode provided");
            }

            uriBuilder.Append(productCode.Trim());

            var uriString = uriBuilder.ToString();

            return await GetPayloadUsingGet<StockItemResponse>(uriString);
        }

        public async Task<ProductItemResponse> GetProductItemById(int productId, bool expandByVariations = false)
        {
            var uriBuilder = new StringBuilder();
            uriBuilder.Append(_baseUrl);
            uriBuilder.Append("/products/");

            uriBuilder.Append(productId);

            if (expandByVariations)
            {
                uriBuilder.Append("?expand=variations");
            }

            var uriString = uriBuilder.ToString();

            return await GetPayloadUsingGet<ProductItemResponse>(uriString);
        }

        public async Task<ProductItemResponse> GetProductItemByProductCode(string productCode, bool expandByVariations = false)
        {
            var uriBuilder = new StringBuilder();
            uriBuilder.Append(_baseUrl);
            uriBuilder.Append("/products/product_code=");
            if (string.IsNullOrWhiteSpace(productCode))
            {
                throw new Exception("No valid ProductCode provided");
            }

            uriBuilder.Append(productCode.Trim());

            if (expandByVariations)
            {
                uriBuilder.Append("?expand=variations");
            }

            var uriString = uriBuilder.ToString();

            return await GetPayloadUsingGet<ProductItemResponse>(uriString);
        }

        public async Task<StockChangeResponse> GetStockChanges(int? stockItemId, DateTimeOffset? changedAtFrom, DateTimeOffset? changedAtTo, int pageSize = 100, int page = 1)
        {
            var uriBuilder = new StringBuilder();
            uriBuilder.Append(_baseUrl);
            uriBuilder.Append("/stock-changes?");
            if (stockItemId != null)
            {
                uriBuilder.Append($"stock_item_id={stockItemId.Value}&");
            }
            if (changedAtFrom != null)
            {
                uriBuilder.Append($"changed_at-from={changedAtFrom.Value:s}&");
            }
            if (changedAtTo != null)
            {
                uriBuilder.Append($"changed_at-to={changedAtTo.Value:s}&");
            }

            uriBuilder.Append($"page_size={pageSize}&");
            uriBuilder.Append($"page={page}&");

            var uriString = uriBuilder.ToString();

            uriString = uriString.Remove(uriString.Length - 1);

            return await GetPayloadUsingGet<StockChangeResponse>(uriString);
        }

        private async Task<TResponse> GetPayloadUsingGet<TResponse>(string uri)
        {
            var client = GetAuthorizedClient();

            using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(uri)))
            {
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                var response = await client.SendAsync(request);

                var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
                if (response.Content != null && response.Content.Headers != null)
                {
                    foreach (var httpContentHeader in response.Content.Headers)
                    {
                        headers[httpContentHeader.Key] = httpContentHeader.Value;
                    }
                }

                var status = $"{(int) response.StatusCode}";
                string errorMsg;

                switch (status)
                {
                    case "200":
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        var deserialized = JsonConvert.DeserializeObject<TResponse>(jsonContent);
                        return deserialized;
                    case "400":
                        errorMsg = "Bad request. Make sure that the request is using the encrypted HTTPS protocol.";
                        break;
                    case "401":
                        errorMsg = "Unauthorized. Check the user name and API key used to authenticate the request.";
                        break;
                    case "404":
                        errorMsg = "Not found.";
                        break;
                    case "406":
                        errorMsg =
                            "Not acceptable. The request did not include the Accept: application/json HTTP header.";
                        break;
                    case "409":
                        errorMsg = "The action cannot be processed, because the item is in an incorrect state.";
                        break;
                    case "500":
                        errorMsg =
                            "Processing the action was interrupted by an error in MyCashflow or an external service.";
                        break;
                    default:
                        errorMsg = $"The HTTP status code of the response was not expected ({status}).";
                        break;
                }

                var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync();
                throw new McfClientException(errorMsg, status, responseData, headers);
            }
        }

        private HttpClient GetAuthorizedClient()
        {
            var client = _client;
            var byteArray = Encoding.ASCII.GetBytes($"{_userName}:{_apiKey}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return client;
        }


    }
}
