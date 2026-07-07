using BayTack.Domain.Entities.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
	{
		public void Configure(EntityTypeBuilder<PaymentMethod> builder)
		{
			builder.Property(p => p.Name).HasMaxLength(50).IsRequired();
			builder.HasIndex(p => p.Name).IsUnique();
		}
	}

}
