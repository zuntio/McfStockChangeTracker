using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McfStockChangeTracker.Extensions;
using McfStockChangeTracker.Models;
using McfStockChangeTracker.Options;
using MyCashFlow.Client;
using MyCashFlow.Client.Exceptions;
using MyCashFlow.Client.Serialization.Responses;

namespace McfStockChangeTracker.Services
{
    public class StockChangeTrackingService
    {
        private readonly McfClient _client;
        private readonly McfStockChangeTrackerUserOptions _userOptions;

        public StockChangeTrackingService(McfStockChangeTrackerApiOptions apiOptions, McfStockChangeTrackerUserOptions userOptions)
        {
            _client = new McfClient(apiOptions.StoreName, apiOptions.ApiUser, apiOptions.ApiKey);
            _userOptions = userOptions;
        }

        public async Task<List<StockChangeDto>> SmartQueryForStockChanges(string productCode, DateTimeOffset? start,
            DateTimeOffset? end)
        {
            try
            {
                var productResponse = await _client.GetProductItemByProductCode(productCode, true);
                var product = productResponse.Data;
                var productName = product.Name;
                if (product.Variations.IsNullOrEmptyCollection())
                {
                    return await GetStockChangesForProductCode(productCode, start, end, productName);
                }
                else
                {
                    var stockChangeDtos = new List<StockChangeDto>();
                    foreach (var productVariation in product.Variations)
                    {
                        var variationData = await GetStockChangesForProductCode
                        (
                            productVariation.ProductCode,
                            start,
                            end,
                            productName,
                            productVariation.Name
                        );
                        stockChangeDtos.AddRange(variationData);
                    }

                    return stockChangeDtos;
                }

            }
            catch (McfClientException ex)
            {
                if (ex.StatusCode == "401")
                    throw new Exception("Ei voitu hakea tietoja. Tarkista käyttäjätunnus ja api-avain");
                if (ex.StatusCode == "404")
                    return await GetStockChangesForVariation(productCode, start, end);
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<StockChangeDto>> GetStockChangesForVariation(string productCode, DateTimeOffset? start, DateTimeOffset? end)
        {
            try
            {
                var stockItemResponse = await _client.GetStockItem(productCode);
                var productId = stockItemResponse.Data.ProductId;
                var variationId = stockItemResponse.Data.VariationId;
                var productWithVariations = await _client.GetProductItemById(productId, true);
                var variation = productWithVariations.Data.Variations.SingleOrDefault(x => x.Id == variationId);
                var stockChangesOfVariation = await GetStockChangesForProductCode(variation.ProductCode, start, end,
                    productWithVariations.Data.Name, variation.Name);
                return stockChangesOfVariation;
            }
            catch (McfClientException ex)
            {
                if (ex.StatusCode == "404")
                    throw new Exception("Tuotetta tai variaatiota ei löydy");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<StockChangeDto>> GetStockChangesForProductCode(string productCode, DateTimeOffset? start, DateTimeOffset? end, string productName, string variationName = "")
        {
            var stockItem = await _client.GetStockItem(productCode);
            var stockItemId = stockItem.Data.Id;
            var stockChangeData = await GetPagedStockChanges(stockItemId, start, end);
            return MapToStockChangeDtos(stockChangeData, productName, variationName);

        }

        private async Task<List<StockChangeData>> GetPagedStockChanges(int stockItemId, DateTimeOffset? start,
            DateTimeOffset? end)
        {
            var stockChangeData = new List<StockChangeData>();
            int page = 1;
            int pageCount;
            do
            {
                var response = await _client.GetStockChanges(stockItemId, start, end, 1000, page);
                pageCount = response.Meta.PageCount;
                stockChangeData.AddRange(response.Data);
                page++;
            } while (page <= pageCount);

            return stockChangeData;
        }

        private List<StockChangeDto> MapToStockChangeDtos(List<StockChangeData> data, string productName, string variationName = "")
        {
            return data.Select(x => new StockChangeDto
            {
                Product = productName,
                Variation = variationName,
                StockChangeType = x.SourceType,
                TimeStamp = x.ChangedAt.ToString("d.M.yyyy hh:mm:ss"),
                User = _userOptions.Users.ContainsKey(x.UserId.ToString()) ? _userOptions.Users[x.UserId.ToString()] : x.UserId.ToString(),
                Differential = x.QuantityChange,
                TargetAmount = x.Quantity
            }).ToList();
        }
    }
}
