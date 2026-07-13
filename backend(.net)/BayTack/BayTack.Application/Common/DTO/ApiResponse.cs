using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.DTO
{
	public class ApiResponse<T>
	{
		public T? Data { get; set; }
		public string? Message { get; set; } 
		public bool IsSuccess { get; set; }
		public string? Errors { get; set; }
		public string? ErrorCode { get; set; }
		public int StatusCode { get; set; }
		public string? TraceId { get; set; }
	}
}
