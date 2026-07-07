
using BayTack.Application.Common.Models;
using MediatR;

namespace BayTack.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

}
