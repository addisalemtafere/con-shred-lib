using Convex.Shared.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Extensions
{
    public static class ResultExtensions
    {
        public static Result<T> Ensure<T>(
            this Result<T> result,
            Func<T, bool> predicate,
            Error error)
        {
            if (result.IsFailure)
                return result;

            return predicate(result.Value)
                ? result
                : Result.Failure<T>(error);
        }

        public static Result<TOut> Map<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, TOut> mappingFunc)
        {
            return result.IsSuccess
                ? Result.Success(mappingFunc(result.Value))
                : Result.Failure<TOut>(result.Errors);
        }

        public static async Task<Result<TOut>> Bind<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, Task<Result<TOut>>> func)
        {
            return result.IsSuccess
                ? await func(result.Value)
                : Result.Failure<TOut>(result.Errors);
        }

        public static TOut Match<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, TOut> onSuccess,
            Func<Error[], TOut> onFailure)
        {
            return result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Errors);
        }
    }
}
