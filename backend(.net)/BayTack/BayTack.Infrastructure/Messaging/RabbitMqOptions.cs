using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Messaging
{
	public sealed class RabbitMqOptions
	{
		public const string SectionName = "RabbitMq";

		public string Host { get; set; } = "localhost";
		public string VirtualHost { get; set; } = "/";
		public string Username { get; set; } = "baytack";
		public string Password { get; set; } = "baytack_dev_pw";
	}
}
