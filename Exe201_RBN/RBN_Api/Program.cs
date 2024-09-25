using RBN_Api.Extensions;
using Repositories.IRepositories;
using Repositories.Repositories;
using Services.Configurations.JWT;
using Services.Configurations.Mapper;
using Services.IService;
using Services.Service;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.Register();
        JWTConfiguration.ConfigureJwt(builder.Services, builder.Configuration);

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}