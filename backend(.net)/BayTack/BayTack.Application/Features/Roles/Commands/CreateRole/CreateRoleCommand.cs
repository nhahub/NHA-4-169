using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using FluentValidation;

namespace BayTack.Application.Features.Roles.Commands.CreateRole
{
	public sealed record CreateRoleCommand(string Name, List<string>? Permissions) : ICommand<RoleResponse>;

	public sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, RoleResponse>
	{
		private readonly IRoleRepository _roles;

		public CreateRoleCommandHandler(IRoleRepository roles) => _roles = roles;

		public async Task<Result<RoleResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
		{
			var role = await _roles.CreateAsync(request.Name, request.Permissions, cancellationToken);
			return role is null
				? Result<RoleResponse>.Failure($"Role '{request.Name}' already exists", statusCode: 409)
				: Result<RoleResponse>.Success(role);
		}
	}

	public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
	{
		public CreateRoleCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
		}
	}
}
