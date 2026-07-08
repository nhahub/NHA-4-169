using BayTack.Application.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Common
{

	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime UtcNow => DateTime.UtcNow;
	}

}
