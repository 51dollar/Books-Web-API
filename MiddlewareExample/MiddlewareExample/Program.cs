namespace MiddlewareExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Привет это middleware компонент.");
            });

            app.MapControllers();

            app.Run();
        }
    }
}
