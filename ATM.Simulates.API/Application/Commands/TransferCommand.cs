using ATM.Simulates.API.Response;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace ATM.Simulates.API.Application.Commands
{
    public class TransferCommand : IRequest<TransferResponse>
    {
        public decimal Amount { get; set; }
        public string AccountSource { get; set; }
        public string AccountDes { get; set; }
        public string Sign { get; set; }


        [IgnoreDataMember]
        public Guid WalletSourceId { get; set; }
        [IgnoreDataMember]
        public Guid WalletDesId { get; set; }
        [IgnoreDataMember]
        public decimal Fee { get; set; }
        [IgnoreDataMember]
        public int WalletTypeId { get; set; }
        [IgnoreDataMember]
        public int TransactionTypeId { get; set; }
    }
}
