using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Users.Queries.GetAllUsers;


namespace BayTack.Application.Features.Users.Queries.GetUserById
{

	public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
	{
		private readonly IUserRepository _userRepository;

		public GetUserByIdQueryHandler(IUserRepository userRepository) => _userRepository = userRepository;

		public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken ct)
		{
			var user = await _userRepository.GetByIdAsync(request.Id, ct);
			return user is not null ? Result<UserResponse>.Success(user) : Result<UserResponse>.Failure("User not found");

		}
	}

}
