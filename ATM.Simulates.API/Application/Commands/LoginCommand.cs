using ATM.Simulates.API.Response;
using MediatR;

namespace ATM.Simulates.API.Application.Commands
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string PinCode { get; set; }
    }
}
