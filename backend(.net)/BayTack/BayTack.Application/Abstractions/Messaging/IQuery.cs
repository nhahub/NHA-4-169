
using BayTack.Application.Common.Models;

namespace BayTack.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

}
