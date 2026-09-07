using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Messaging
{
	public sealed class RabbitMqOptions
	{
		public const string SectionName = "RabbitMq";

		public string Host { get; set; } 
		public string VirtualHost { get; set; }
		public string Username { get; set; } 
		public string Password { get; set; }
	}
}
