using AutoMapper;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Refit;
using ScoreWorker.DB;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Domain;
using ScoreWorker.Domain.Interfaces;
using ScoreWorker.Infrastructure.Mapping;
using ScoreWorker.Infrastructure.Middlewares;
using ScoreWorker.Prompt;
using ScoreWorker.Prompt.Interfaces;
using ScoreWorker.RefitApi;

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

        services.AddDbContext<ScoreWorkerDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("SQLConnectionString"));
        });

        services.AddSingleton(new MapperConfiguration(mc =>
        {
            mc.AddProfile<MappingProfile>();
        }).CreateMapper());

        services.AddControllers();

        services.AddRefitClient<IVkControllerApi>();

        services.AddScoped<IDataProvider, ScoreWorkerDbContext>();

        services.AddScoped<IPromptHandler, PromptHandler>();
        services.AddScoped<IPromptParser, PromptParser>();

        services.AddScoped<ITestSolution, TestSolution>();
        services.AddScoped<IScoreWorkerService, ScoreWorkerService>();

        services.AddHttpContextAccessor();

        services.AddSwaggerGen();

        services.AddHangfire(options =>
        {
            options.UsePostgreSqlStorage(optPostgres =>
            {
                optPostgres.UseNpgsqlConnection(Configuration.GetConnectionString("SQLConnectionString"));
            });
            options.UseFilter(new AutomaticRetryAttribute
            {
                Attempts = 0
            });
        });

        services.AddHangfireServer();
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

        app.UseMiddleware<GlobalExceptionMiddleware>();

        UpdateDatabase(app);

        app.UseRouting();
        app.UseCors("CorsPolicy");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHangfireDashboard();
            endpoints.MapControllers().RequireCors("CorsPolicy");
        });

        app.UseHangfireDashboard();
    }

    private void UpdateDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope.ServiceProvider
            .GetService<ScoreWorkerDbContext>();

        context!.Database.Migrate();
    }
}