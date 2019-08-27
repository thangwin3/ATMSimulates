using ATM.Simulates.API.Application.Queries.Request;
using ATM.Simulates.API.Response;
using System.Threading.Tasks;

namespace ATM.Simulates.API.Application.Queries
{
    public interface IAccountQueries
    {
        Task<GetListAccountResponse> GetListAccount();
        Task<GetListTransactionResponse> GetListTransactionAsync();
        Task<GetBalanceResponse> GetBalance(GetBalanceRequest request);
        Task<GetTransactionDetailResponse> GetTransactionDetail(GetTransactionDetailRequest request);
    }
}
