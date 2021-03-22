using Polly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityLibrary.PollyProject
{
    public static class NonHttpPolicy
    {
        public static AsyncPolicy CreateRetryPolicy()
        {
            return Policy
                    .Handle<ArgumentOutOfRangeException>()
                    .Or<IndexOutOfRangeException>()
                    .Or<ArgumentException>()
                    .WaitAndRetryAsync(3,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (ex, count) =>
                        {
                            Console.WriteLine($"{ex}, {count}");
                        });
        }

        public static AsyncPolicy CreateRetryPolicy(List<Exception> exceptions = null)
        {
            exceptions ??= new List<Exception>();
            return Policy
                    .Handle<Exception>(ex => IsExceptionPresent(ex, exceptions))
                    .WaitAndRetryAsync(3,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)),
                        (ex, count) =>
                        {
                            Console.WriteLine($"{ex}, {count}");
                        });
        }

        private static bool IsExceptionPresent(Exception exceptionOccured, List<Exception> userDefinedExceptions)
        {
            return userDefinedExceptions.Any(ex => ex.GetType() == exceptionOccured.GetType());
        }
    }
}