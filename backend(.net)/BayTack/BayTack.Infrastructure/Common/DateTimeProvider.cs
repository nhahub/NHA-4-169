using BayTack.Application.Abstractions.Interfaces;

namespace BayTack.Infrastructure.Common
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime UtcNow => DateTime.UtcNow;
	}
}
