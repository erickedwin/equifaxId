using Microsoft.EntityFrameworkCore;
using equifaxIdWin.Context;
using Microsoft.AspNetCore.Builder;
using equifaxIdWin.Services;
using equifaxIdWin.Repository;

namespace equifaxIdWin;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        string connectionString = Environment.GetEnvironmentVariable("AWS_WIN");
 
        //string coneccion = Environment.GetEnvironmentVariable("awsWin")!;
        //var connectionString = Configuration.GetConnectionString("awsWin");
        services.AddDbContext<awsEquifaxContext>(options =>
        options.UseNpgsql(connectionString));

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddTransient<IequifaxId, equifaxIdRP>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Servicio Construido por la empresa Yacuruna");
            });
        });
    }
}