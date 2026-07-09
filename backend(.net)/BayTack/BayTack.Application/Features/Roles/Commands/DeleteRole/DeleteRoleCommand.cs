using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Roles.Commands.DeleteRole
{
	public sealed record DeleteRoleCommand(string Id) : ICommand;

	public sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
	{
		private readonly IRoleRepository _roles;

		public DeleteRoleCommandHandler(IRoleRepository roles) => _roles = roles;

		public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
		{
			var (success, error) = await _roles.DeleteAsync(request.Id, cancellationToken);
			if (success) return Result.Success();

			return error == "Role not found" ? Result.NotFound(error) : Result.Failure(error ?? "Failed to delete role");
		}
	}
}
