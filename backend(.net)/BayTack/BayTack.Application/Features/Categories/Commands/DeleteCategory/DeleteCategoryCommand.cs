using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Categories.Commands.DeleteCategory
{
	public sealed record DeleteCategoryCommand(string Id) : ICommand;
}
