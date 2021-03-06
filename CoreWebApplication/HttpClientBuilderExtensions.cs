﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace CoreWebApplication
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddResilientPolicy<TResilientPolicy>(this IHttpClientBuilder builder)
            where TResilientPolicy : class, IResilientPolicy
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddSingleton<TResilientPolicy>();
            builder.AddPolicyHandler((sp, request) => sp.GetRequiredService<TResilientPolicy>().Policy);

            return builder;
        }
    }
}