using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Categories.Commands.UpdateCategory
{
	public sealed record UpdateCategoryCommand(string Id, string? Name, string? Icon, string? Description) : ICommand<CategoryResponse>;
}
