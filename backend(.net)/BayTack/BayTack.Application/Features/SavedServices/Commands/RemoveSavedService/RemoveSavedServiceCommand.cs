using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.SavedServices.Commands.RemoveSavedService
{
    public sealed record RemoveSavedServiceCommand(string CustomerId, string ServiceId) : ICommand;
}