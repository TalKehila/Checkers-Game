var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IGameService, GameService>();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
	{
		builder.WithOrigins("http://localhost:3000")
			   .AllowAnyHeader()
			   .AllowAnyMethod()
			   .AllowCredentials();
	});
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<CheckersHub>("/checkersHub");

app.Run();