using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Refit;
using SampleSolution.Services;
using SampleSolution.Services.Interfaces;
using ScoreWorker.Mapping;
using ScoreWorker.RefitApi;
using ScoreWorkerDB;

namespace ScoreWorker;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc().AddNewtonsoftJson();

        services
            .AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });


        services.AddDbContext<ReviewDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("SQLConnectionString"));
        });

        services.AddSingleton(new MapperConfiguration(mc =>
        {
            mc.AddProfile<MappingProfile>();
        }).CreateMapper());

        services.AddControllers();

        services.AddHttpContextAccessor();
        services.AddRefitClient<IVkControllerApi>();

        services.AddScoped<ITestSolution, TestSolution>();
        services.AddScoped<IScoreWorkerService, ScoreWorkerService>();

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseHsts();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        UpdateDatabase(app);

        app.UseRouting();
        app.UseCors("CorsPolicy");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers().RequireCors("CorsPolicy");
        });
    }

    private void UpdateDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope.ServiceProvider
            .GetService<ReviewDbContext>();

        context!.Database.Migrate();
    }
}