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

<<<<<<< HEAD
        builder.Services.AddDbContext<IApplicationDbContext, ApplicationDBContext>();

        builder.Services.AddAutoMapper(typeof(MappingEntities));
=======

>>>>>>> a27f7777a74770acc597d4937507af42cd6bb39b
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