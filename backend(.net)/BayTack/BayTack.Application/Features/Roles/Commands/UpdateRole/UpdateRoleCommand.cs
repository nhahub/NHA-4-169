using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using FluentValidation;

namespace BayTack.Application.Features.Roles.Commands.UpdateRole
{
	public sealed record UpdateRoleCommand(string Id, string Name) : ICommand<RoleResponse>;

	public sealed class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, RoleResponse>
	{
		private readonly IRoleRepository _roles;

		public UpdateRoleCommandHandler(IRoleRepository roles) => _roles = roles;

		public async Task<Result<RoleResponse>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
		{
			var role = await _roles.RenameAsync(request.Id, request.Name, cancellationToken);
			return role is null
				? Result<RoleResponse>.NotFound($"Role '{request.Id}' not found")
				: Result<RoleResponse>.Success(role);
		}
	}

	public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
	{
		public UpdateRoleCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
		}
	}
}
