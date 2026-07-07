using BayTack.Application.Common.DTO;
using BayTack.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Extensions
{
	public static class ResultExtensions
	{
		public static ApiResponse<T> ToApiResponse<T>(this Result<T> result, int successStatusCode = 200)
		{
			if (result.IsSuccess)
			{
				return new ApiResponse<T>
				{
					IsSuccess = true,
					Data = result.Value,
					StatusCode = successStatusCode
				};
			}

			return new ApiResponse<T>
			{
				IsSuccess = false,
				Errors = result.Error,
				StatusCode = 400
			};
		}
	}
}
