
using BayTack.Application.Common.Models;
using MediatR;

namespace BayTack.Application.Abstractions.Messaging
{
 
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
    {

    }
}
