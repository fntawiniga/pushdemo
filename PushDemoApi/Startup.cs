using PushDemoApi.Authentication;
using PushDemoApi.Models;
using PushDemoApi.Services;

namespace PushDemoApi
{
    public class Startup
    {
        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }
        public IConfigurationRoot Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthOptions.DefaultScheme;
            }).AddApiKeyAuth(Configuration.GetSection("Authentication").Bind);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            services.AddSingleton<INotificationService, NotificationHubService>();

            services.AddOptions<NotificationHubOptions>()
                .Configure(Configuration.GetSection("NotificationHub").Bind)
                .ValidateDataAnnotations();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
