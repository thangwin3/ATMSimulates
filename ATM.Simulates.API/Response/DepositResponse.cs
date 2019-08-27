namespace ATM.Simulates.API.Response
{
    public class DepositResponse :BaseResponse
    {

        public DepositData Data { get; set; }
        public DepositResponse()
        {
            this.Data = new DepositData();
        }
    }
    public class DepositData
    {
        public decimal Balance { get; set; }
        public long TransactionId { get; set; }
    }
}
