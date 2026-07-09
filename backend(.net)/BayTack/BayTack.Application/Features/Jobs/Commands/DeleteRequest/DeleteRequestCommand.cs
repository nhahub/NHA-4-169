using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Jobs.Commands.DeleteRequest
{
    public sealed record DeleteRequestCommand(string JobId, string CustomerId) : ICommand;
}