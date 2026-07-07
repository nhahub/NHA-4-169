
using BayTack.Application.Common.Models;
using MediatR;

namespace BayTack.Application.Abstractions.Messaging
{
    public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
}
