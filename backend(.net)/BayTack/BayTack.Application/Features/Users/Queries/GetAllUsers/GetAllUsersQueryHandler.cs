using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Queries.GetAllUsers
{

	public sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, PaginatedList<UserResponse>>
	{
		private readonly IUserRepository _userRepository;

		public GetAllUsersQueryHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<Result<PaginatedList<UserResponse>>> Handle(
			GetAllUsersQuery request, CancellationToken cancellationToken)
		{
			var (items, totalCount) = await _userRepository.SearchAsync(
				request.Search, request.Role, request.Page, request.Limit, cancellationToken);

			return new PaginatedList<UserResponse>(items.ToList(), totalCount, request.Page, request.Limit);
		}
	}
}
