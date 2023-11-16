using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FatCat.Toolkit.WebServer.Binders;

[ExcludeFromCodeCoverage(Justification = "Helper too simple to test")]
public class ToolkitBinderFilterAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		foreach (var value in context.ActionArguments.Values)
		{
			if (value is not IToolkitBinder binder)
			{
				continue;
			}

			if (binder.Result.IsUnsuccessful)
			{
				context.Result = binder.Result;
			}

			return;
		}
	}
}
