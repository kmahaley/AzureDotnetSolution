using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UtilityLibrary.PollyProject
{
    public static class NonHttpPolicy
    {
        public static AsyncPolicy CreateRetryPolicy() {

            return Policy
                    .Handle<ArgumentOutOfRangeException>()
                    .Or<IndexOutOfRangeException>()
                    .WaitAndRetryAsync(3, 
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (ex, count) => {
                            Console.WriteLine($"{ex}, {count}");
                        });
        }
    }
}
