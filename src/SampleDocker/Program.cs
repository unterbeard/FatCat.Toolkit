using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FatCat.Toolkit.Injection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SampleDocker;

public static class Program
{
	public static void Main(params string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddControllers();

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddCors(options =>
									options.AddDefaultPolicy(p =>
																p.AllowAnyOrigin()));

		// Call UseServiceProviderFactory on the Host sub property 
		builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
		
		// Call ConfigureContainer on the Host sub property 
		// Register services directly with Autofac here. Don't
		// call builder.Populate(), that happens in AutofacServiceProviderFactory.
		builder.Host.ConfigureContainer<ContainerBuilder>((a, b) => SystemScope.Initialize(b, new List<Assembly> { typeof(Program).Assembly }));

		var app = builder.Build();

		app.UseHttpsRedirection();

		// app.UseHttpsRedirection();
		app.UseCors();

		app.UseAuthorization();

		app.MapControllers();

		SystemScope.Container.LifetimeScope = app.Services.GetAutofacRoot();

		app.Run();
	}
}