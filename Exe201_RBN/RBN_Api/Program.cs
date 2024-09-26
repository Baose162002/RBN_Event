using BusinessObject.DTO;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using RBN_Api.Extensions;
using Repositories.IRepositories;
using Repositories.Repositories;
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
		var configuration = builder.Configuration;

		// Configure CloudinarySettings and Cloudinary service
		builder.Services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
		builder.Services.AddSingleton<Cloudinary>(sp =>
		{
			var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
			var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
			return new Cloudinary(account);
		});

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