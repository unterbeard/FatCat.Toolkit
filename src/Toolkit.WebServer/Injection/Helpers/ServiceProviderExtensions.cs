// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Globalization;
using Autofac;

namespace FatCat.Toolkit.WebServer.Injection.Helpers;

/// <summary>
/// Extension methods for use with the <see cref="AutofacServiceProvider"/>.
/// </summary>
public static class ServiceProviderExtensions
{
	/// <summary>
	/// Tries to cast the instance of <see cref="ILifetimeScope"/> from <see cref="AutofacServiceProvider"/> when possible.
	/// </summary>
	/// <param name="serviceProvider">The instance of <see cref="IServiceProvider"/>.</param>
	/// <exception cref="InvalidOperationException">Throws an <see cref="InvalidOperationException"/> when instance of <see cref="IServiceProvider"/> can't be assigned to <see cref="AutofacServiceProvider"/>.</exception>
	/// <returns>Returns the instance of <see cref="ILifetimeScope"/> exposed by <see cref="AutofacServiceProvider"/>.</returns>
	public static ILifetimeScope GetAutofacRoot(this IServiceProvider serviceProvider)
	{
		if (serviceProvider is not AutofacServiceProvider autofacServiceProvider)
		{
			throw new InvalidOperationException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Unable to retrieve Autofac root lifetime scope from service provider of type {0}.",
					serviceProvider?.GetType()
				)
			);
		}

		return autofacServiceProvider.LifetimeScope;
	}
}
