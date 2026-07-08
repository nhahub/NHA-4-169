using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using MediatR;
using System.Windows.Input;

namespace BayTack.Application.Common.Behaviors
{
    /// <summary>
    /// After a handler returns a non-error Result, persists changes for ICommand / ICommand&lt;T&gt;
    /// requests only (queries never write, so they're left alone). Command handlers must NOT call
    /// IUnitOfWork.SaveChangesAsync themselves - this behavior is the single place that does it.
    /// </summary>
    public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<TResponse> Handle(
            TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var response = await next();

            var isCommand = request is System.Windows.Input.ICommand || typeof(TRequest).GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>));

            if (isCommand && response is IResult { IsSuccess: true })
                await _unitOfWork.SaveChangesAsync(ct);

            return response;
        }
    }
}