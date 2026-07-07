using BayTack.Domain.Entities.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class ServicePaymentMethodConfiguration : IEntityTypeConfiguration<ServicePaymentMethod>
	{
		public void Configure(EntityTypeBuilder<ServicePaymentMethod> builder)
		{
			builder.HasOne<PaymentMethod>().WithMany().HasForeignKey(s => s.PaymentMethodId).OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(s => new { s.ServiceId, s.PaymentMethodId }).IsUnique();
		}
	}

}
