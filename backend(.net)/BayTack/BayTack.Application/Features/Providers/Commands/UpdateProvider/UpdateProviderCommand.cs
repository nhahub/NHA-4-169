using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.UpdateProvider
{
	public sealed record UpdateProviderCommand(string Id, string? ProviderType, int? YearsOfExperience, string? CategoryId) : ICommand<ProviderResponse>;

	public sealed class UpdateProviderCommandHandler : ICommandHandler<UpdateProviderCommand, ProviderResponse>
	{
		private readonly IProviderRepository _providers;

		public UpdateProviderCommandHandler(IProviderRepository providers) => _providers = providers;

		public async Task<Result<ProviderResponse>> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
		{
			var provider = await _providers.UpdateAsync(request.Id, request.ProviderType, request.YearsOfExperience, request.CategoryId, cancellationToken);
			return provider is null
				? Result<ProviderResponse>.NotFound($"Provider '{request.Id}' not found")
				: Result<ProviderResponse>.Success(provider);
		}
	}

	public sealed class UpdateProviderCommandValidator : AbstractValidator<UpdateProviderCommand>
	{
		public UpdateProviderCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).When(x => x.YearsOfExperience.HasValue);
		}
	}
}
