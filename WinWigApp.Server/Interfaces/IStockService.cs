using WinWigApp.Server.DTOs;
using WinWigApp.Server.Models;

namespace WinWigApp.Server.Interfaces
{
    public interface IStockService
    {
        Task<List<StockResponse>> GetStocksAsync();
        Task<List<CandlestickData>> GetCandlestickDataAsync(string symbol, int days);
        Task<TechnicalIndicatorsResponse> GetTechnicalIndicatorsAsync(string symbol, int days);
        Task<IEnumerable<Stock>> GetRealTimeStocksAsync();
    }
}