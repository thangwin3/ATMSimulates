using ATM.Simulates.API.Response;
using MediatR;
using System.Runtime.Serialization;

namespace ATM.Simulates.API.Application.Commands
{
    public class WithdrawCommand : IRequest<WithdrawResponse>
    {
        public decimal Amount { get; set; }

        [IgnoreDataMember]
        public decimal Fee { get; set; }
        [IgnoreDataMember]
        public int WalletTypeId { get; set; }
        [IgnoreDataMember]
        public int TransactionTypeId { get; set; }
    }
}
