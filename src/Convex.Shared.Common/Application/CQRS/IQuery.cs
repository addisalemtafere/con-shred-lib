using Convex.Shared.Common.Models;
using MediatR;

namespace Convex.Shared.Common.Application.CQRS
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    { }

    // Paginated queries
    public interface IPaginatedQuery<TResponse> : IQuery<PaginatedResponse<TResponse>>
    {
        int PageNumber { get; }
        int PageSize { get; }
    }

    public interface IPaginatedQueryHandler<TQuery, TResponse>
        : IQueryHandler<TQuery, PaginatedResponse<TResponse>>
        where TQuery : IPaginatedQuery<TResponse>
    { }
}
