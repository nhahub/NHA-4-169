using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Users.Queries.GetAllUsers;


namespace BayTack.Application.Features.Users.Command.UpdateUser
{
	public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserResponse>
	{
		private readonly IIdentityService _identityService;
		private readonly IUserRepository _userQueryRepository;

		public UpdateUserCommandHandler(IIdentityService identityService, IUserRepository userQueryRepository)
		{
			_identityService = identityService;
			_userQueryRepository = userQueryRepository;
		}

		public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken ct)
		{
			var updateResult = await _identityService.UpdateUserAsync(
				request.Id, request.Name, request.Email, request.Phone, request.Role, request.UpdatedBy, ct);

			if (!updateResult.IsSuccess)
				return Result<UserResponse>.Failure(updateResult.Error!);

			var user = await _userQueryRepository.GetByIdAsync(request.Id, ct);
			return user is not null ? Result<UserResponse>.Success(user) : Result<UserResponse>.Failure("User not found");
		}
	}

}
