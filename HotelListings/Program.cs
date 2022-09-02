using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Serilog logger
builder.Host.UseSerilog((ctx, lc) => lc
	.WriteTo.File(
		path: "c:\\hotellistings\\logs\\log-.txt",
		outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zz} [{Level:u3}] {Message:lj}{NewLine}{Exception}}",
		rollingInterval: RollingInterval.Day,
		restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
	));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
	o.AddPolicy("AllowAll", builder =>
			builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
});


try
{
	Log.Information("Application Is Starting");
	
	var app = builder.Build();

	// Configure the HTTP request pipeline.

	app.UseSwagger();
	app.UseSwaggerUI();


	app.UseHttpsRedirection();

	app.UseCors("AllowAll");

	app.UseAuthorization();

	app.MapControllers();

	app.Run();
	
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application Failed to start");
}
finally
{
	Log.CloseAndFlush();
}

