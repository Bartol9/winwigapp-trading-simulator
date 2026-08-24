using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WinWigApp.Server.Interfaces;
using WinWigApp.Server.Models;

namespace WinWigApp.Server.Services
{
    public class YahooFinanceService : IMarketDataProvider
    {
        private readonly HttpClient _httpClient;

        public YahooFinanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Stock>> GetStockDataAsync(IEnumerable<string> tickers)
        {
            var stocks = new List<Stock>();

            try
            {
                var symbols = string.Join(",", tickers);

                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"https://query1.finance.yahoo.com/v7/finance/quote?symbols={symbols}"
                );

                request.Headers.Add(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64)"
                );

                var response = await _httpClient.SendAsync(request);
                Console.WriteLine($"STATUS: {(int)response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine(content);

                var json = JsonDocument.Parse(content);

                var results = json
                    .RootElement
                    .GetProperty("quoteResponse")
                    .GetProperty("result");

                foreach (var result in results.EnumerateArray())
                {
                    stocks.Add(new Stock
                    {
                        Symbol = result.GetProperty("symbol").GetString(),

                        Name = result.TryGetProperty("shortName", out var shortName)
                            ? shortName.GetString()
                            : "",

                        CurrentPrice = result.TryGetProperty("regularMarketPrice", out var price)
                            ? price.GetDecimal()
                            : 0,

                        ChangePercent = result.TryGetProperty("regularMarketChangePercent", out var cp)
                            ? cp.GetDecimal()
                            : 0,

                        Volume = result.TryGetProperty("regularMarketVolume", out var vol)
                            ? vol.GetInt64()
                            : 0
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return stocks;
        }
    }
}