using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Hosting;
using WinWigApp.Server.Interfaces;
using WinWigApp.Server.Models;

namespace WinWigApp.Server.Services
{
    public class StockMarketBackgroundService : BackgroundService
    {
        private readonly IMarketDataProvider _marketDataProvider;
        private readonly List<Stock> _cachedStocks = new();
        private readonly string[] _tickers = { "AAPL", "MSFT", "NVDA", "AMZN", "META", "TSLA", "GOOGL" };
        private readonly string _csvFilePath = "stock_data.csv";

        public StockMarketBackgroundService(IMarketDataProvider marketDataProvider)
        {
            _marketDataProvider = marketDataProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var stocks = await _marketDataProvider.GetStockDataAsync(_tickers);
                    _cachedStocks.Clear();
                    _cachedStocks.AddRange(stocks);

                    SaveToCsv(stocks);
                }
                catch (Exception ex)
                {
                    // Log error (placeholder)
                    Console.WriteLine($"Error in background service: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }

        private void SaveToCsv(IEnumerable<Stock> stocks)
        {
            try
            {
                using var writer = new StreamWriter(_csvFilePath, false, Encoding.UTF8);
                using var csv = new CsvWriter(writer, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture));
                csv.WriteRecords(stocks);
            }
            catch (Exception ex)
            {
                // Log error (placeholder)
                Console.WriteLine($"Error saving to CSV: {ex.Message}");
            }
        }

        public IEnumerable<Stock> GetCachedStocks() => _cachedStocks;
    }
}