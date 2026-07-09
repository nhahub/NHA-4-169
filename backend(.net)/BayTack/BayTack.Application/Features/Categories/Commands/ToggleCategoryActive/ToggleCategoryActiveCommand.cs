using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Categories.Commands.ToggleCategoryActive
{
	public sealed record ToggleCategoryActiveCommand(string Id) : ICommand<CategoryResponse>;
}
