
using BayTack.Application.Common.Models;

namespace BayTack.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest<Result> { }
    public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
}
