using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.SavedServices.Commands.AddSavedService
{
    public sealed record AddSavedServiceCommand(string CustomerId, string ServiceId) : ICommand;
}