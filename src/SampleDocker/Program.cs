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

		var app = builder.Build();

		// app.UseHttpsRedirection();
		app.UseCors();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}