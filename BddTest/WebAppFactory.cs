using Business;
using Business.Entity;
using Business.Repository;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public class WebAppFactory : WebApplicationFactory<Program>
{

    public ProductRepository ProductRepository;

    private AppDbContext _appDbContext;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Usuwanie starej konfiguracji AppDbContext (jeśli istnieje)
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(
                option =>
                {
                    var pgconn = "Server=192.168.100.110;Database=testappdb;Port=5432;User Id=dbuser;Password=dbpass;";
                    option.UseLazyLoadingProxies();
                    option.UseNpgsql(pgconn);
                }, ServiceLifetime.Transient);

            // Zbudowanie providera i inicjalizacja bazy danych
            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                _appDbContext = sp.GetRequiredService<AppDbContext>();
                ProductRepository = sp.GetRequiredService<ProductRepository>();

                _appDbContext.Database.EnsureDeleted();
                _appDbContext.Database.EnsureCreated();
            }


        });

        builder.UseEnvironment("Development");
    }

    public async Task InitProductDB(IEnumerable<Product> products) 
    {
        _appDbContext.Database.EnsureDeleted();
        _appDbContext.Database.EnsureCreated();

        foreach (var product in products)
        {
            await ProductRepository.AddAsync(product);
        }
    }
}
