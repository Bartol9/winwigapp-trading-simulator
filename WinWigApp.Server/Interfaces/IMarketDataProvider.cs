using System.Collections.Generic;
using System.Threading.Tasks;
using WinWigApp.Server.Models;

namespace WinWigApp.Server.Interfaces
{
    public interface IMarketDataProvider
    {
        Task<IEnumerable<Stock>> GetStockDataAsync(IEnumerable<string> tickers);
    }
}