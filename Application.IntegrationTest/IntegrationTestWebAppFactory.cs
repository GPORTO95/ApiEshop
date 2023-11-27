using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application.IntegrationTest;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            // Incluir conexão de banco de test
            // Ou em caso de container utilizar o pacote Testcontainers.PostgreSql
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseSqlServer("Server=localhost,1433;Database=PublicEnterpriseDb;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=true");
            });

        });
    }
}
