using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ServiceAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Services.Commands.DeleteService
{
	public sealed class DeleteServiceCommandHandler : ICommandHandler<DeleteServiceCommand>
	{
		private readonly IRepository<Service, string> _serviceRepository;
		public DeleteServiceCommandHandler(
			IRepository<Service, string> serviceRepository )
		{
			_serviceRepository = serviceRepository;
		}
		public async Task<Result> Handle(DeleteServiceCommand request, CancellationToken ct)
		{
			var service = await _serviceRepository.GetByIdAsync(request.Id, ct);
			if (service is null)
				return Result.Failure("Service not found.");
			_serviceRepository.Remove(service);

			return Result.Success();
		}
	}
}
