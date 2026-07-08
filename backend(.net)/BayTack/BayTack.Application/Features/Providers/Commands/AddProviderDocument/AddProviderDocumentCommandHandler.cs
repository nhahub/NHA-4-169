using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.AddProviderDocument
{
	public sealed class AddProviderDocumentCommandHandler
		: ICommandHandler<AddProviderDocumentCommand, AddProviderDocumentResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;
		private readonly IUnitOfWork _unitOfWork;

		public AddProviderDocumentCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<AddProviderDocumentResponse>> Handle(
			AddProviderDocumentCommand request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.GetByIdAsync(request.ProviderProfileId, ct);

			if (profile is null)
				return Result<AddProviderDocumentResponse>.Failure("Provider profile not found.");

			var document = profile.AddDocument(request.DocType, request.DocUrl);

			_providerProfileRepository.Update(profile);
			await _unitOfWork.SaveChangesAsync(ct);

			var response = new AddProviderDocumentResponse(
				profile.Id,
				document.Id,
				document.DocType,
				document.DocUrl,
				document.Status.ToString());

			return Result<AddProviderDocumentResponse>.Success(response);
		}
	}
}
