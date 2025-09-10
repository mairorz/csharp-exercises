using Microsoft.EntityFrameworkCore;
using WebApplicationServices.Models;

namespace WebApplicationUsers.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    public DbSet<Client> Clients => Set<Client>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<Invoice> Invoices => Set<Invoice>();

    public DbSet<InvoiceDetail> InvoiceDetails => Set<InvoiceDetail>();

    public DbSet<Supplier> Suppliers => Set<Supplier>();

    public DbSet<InputsCategories> InputsCategories => Set<InputsCategories>();

    public DbSet<Inputs> Inputs => Set<Inputs>();

    public DbSet<Purchases> Purchases => Set<Purchases>();

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();

    public DbSet<Consumption> Consumptions => Set<Consumption>();
    
}