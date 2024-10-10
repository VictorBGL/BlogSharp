namespace BlogSharp.Api.Configuration
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddApiConfig(this WebApplicationBuilder builder) 
        {
            builder.Services.AddControllers();

            return builder;
        }
    }
}
