using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Users.Queries.GetAllUsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Command
{
	public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserResponse>
	{
		private readonly IIdentityService _identityService;
		private readonly IUserRepository _userRepository;

		public CreateUserCommandHandler(IIdentityService identityService, IUserRepository userRepository)
		{
			_identityService = identityService;
			_userRepository = userRepository;
		}

		public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken ct)
		{
			var createResult = await _identityService.CreateUserAsync(request.Name, request.Email, request.Phone, request.Role, ct);
			if (!createResult.IsSuccess)
				return Result<UserResponse>.Failure(createResult.Error!);

			var user = await _userRepository.GetByIdAsync(createResult.Value!, ct);
			return user is not null ? Result<UserResponse>.Success(user) : Result<UserResponse>.Failure("User not found.");
		}
	}
}
