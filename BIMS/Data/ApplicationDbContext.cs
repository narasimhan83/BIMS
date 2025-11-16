using BIMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BIMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Master Data DbSets
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<Bank> Banks { get; set; }

        // Calculation Management DbSets
        public DbSet<CalculationMethod> CalculationMethods { get; set; }

        // Location Management DbSets
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        // Agent Management DbSets
        public DbSet<Agent> Agents { get; set; }
        public DbSet<AgentBankDetail> AgentBankDetails { get; set; }

        // Customer Management DbSets
        public DbSet<Group> Groups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerDocument> CustomerDocuments { get; set; }
        public DbSet<CustomerVehicle> CustomerVehicles { get; set; }

        // Vehicle Management DbSets
        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<VehicleYear> VehicleYears { get; set; }
        public DbSet<EngineCapacity> EngineCapacities { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }

        // Insurance Management DbSets
        public DbSet<InsuranceClient> InsuranceClients { get; set; }
        public DbSet<InsuranceClientBankDetail> InsuranceClientBankDetails { get; set; }
        public DbSet<InsurancePlan> InsurancePlans { get; set; }
        public DbSet<LineOfBusiness> LinesOfBusiness { get; set; }
        public DbSet<ProductClass> ProductClasses { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        // CRM DbSets
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Proposal> Proposals { get; set; }

        // Policy Management DbSets
        public DbSet<Policy> Policies { get; set; }

        // Sales DbSets
        public DbSet<SalesType> SalesTypes { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }
        public DbSet<CreditNote> CreditNotes { get; set; }
        public DbSet<CustomerPayment> CustomerPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Customer Types (Individual, Company, Group)
            builder.Entity<CustomerType>().HasData(
                new CustomerType { Id = 1, Name = "Individual", Description = "Individual customer type", IsActive = true, CreatedDate = DateTime.UtcNow },
                new CustomerType { Id = 2, Name = "Company", Description = "Company customer type", IsActive = true, CreatedDate = DateTime.UtcNow },
                new CustomerType { Id = 3, Name = "Group", Description = "Group customer type", IsActive = true, CreatedDate = DateTime.UtcNow }
            );

            // Configure ProductClass relationships
            builder.Entity<ProductClass>()
                .HasOne(pc => pc.BusinessType)
                .WithMany(bt => bt.ProductClasses)
                .HasForeignKey(pc => pc.BusinessTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ProductType relationships
            builder.Entity<ProductType>()
                .HasOne(pt => pt.ProductClass)
                .WithMany(pc => pc.ProductTypes)
                .HasForeignKey(pt => pt.ProductClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure InsuranceClient relationships
            builder.Entity<InsuranceClient>()
                .HasOne(ic => ic.Country)
                .WithMany()
                .HasForeignKey(ic => ic.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InsuranceClient>()
                .HasOne(ic => ic.State)
                .WithMany()
                .HasForeignKey(ic => ic.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InsuranceClient>()
                .HasOne(ic => ic.City)
                .WithMany()
                .HasForeignKey(ic => ic.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure InsuranceClientBankDetail relationships
            builder.Entity<InsuranceClientBankDetail>()
                .HasOne(icbd => icbd.InsuranceClient)
                .WithMany(ic => ic.BankDetails)
                .HasForeignKey(icbd => icbd.InsuranceClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InsuranceClientBankDetail>()
                .HasOne(icbd => icbd.Bank)
                .WithMany()
                .HasForeignKey(icbd => icbd.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Country-State relationships
            builder.Entity<State>()
                .HasOne(s => s.Country)
                .WithMany(c => c.States)
                .HasForeignKey(s => s.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure State-City relationships
            builder.Entity<City>()
                .HasOne(c => c.State)
                .WithMany(s => s.Cities)
                .HasForeignKey(c => c.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Agent relationships
            builder.Entity<Agent>()
                .HasOne(a => a.Country)
                .WithMany()
                .HasForeignKey(a => a.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Agent>()
                .HasOne(a => a.State)
                .WithMany()
                .HasForeignKey(a => a.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Agent>()
                .HasOne(a => a.City)
                .WithMany()
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure AgentBankDetail relationships
            builder.Entity<AgentBankDetail>()
                .HasOne(abd => abd.Agent)
                .WithMany(a => a.BankDetails)
                .HasForeignKey(abd => abd.AgentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AgentBankDetail>()
                .HasOne(abd => abd.Bank)
                .WithMany()
                .HasForeignKey(abd => abd.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Lead relationships
            builder.Entity<Lead>()
                .HasOne(l => l.Customer)
                .WithMany()
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Lead-Proposal relationships
            builder.Entity<Proposal>()
                .HasOne(p => p.Lead)
                .WithMany(l => l.Proposals)
                .HasForeignKey(p => p.LeadId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Lead-Customer relationships
            builder.Entity<Lead>()
                .HasOne(l => l.Customer)
                .WithMany()
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Lead-Customer relationships
            builder.Entity<Lead>()
                .HasOne(l => l.Customer)
                .WithMany()
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Customer-Agent relationships
            builder.Entity<Customer>()
                .HasOne(c => c.Agent)
                .WithMany()
                .HasForeignKey(c => c.AgentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Policy relationships
            builder.Entity<Policy>()
                .HasOne(p => p.InsuranceClient)
                .WithMany()
                .HasForeignKey(p => p.InsuranceClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Policy>()
                .HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure SalesInvoice relationships
            builder.Entity<SalesInvoice>()
                .HasOne(si => si.Customer)
                .WithMany()
                .HasForeignKey(si => si.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure SalesInvoiceItem relationships
            builder.Entity<SalesInvoiceItem>()
                .HasOne(sii => sii.SalesInvoice)
                .WithMany(si => si.Items)
                .HasForeignKey(sii => sii.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure CreditNote relationships
            builder.Entity<CreditNote>()
                .HasOne(cn => cn.SalesInvoice)
                .WithMany(si => si.CreditNotes)
                .HasForeignKey(cn => cn.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure CustomerPayment relationships
            builder.Entity<CustomerPayment>()
                .HasOne(cp => cp.Customer)
                .WithMany()
                .HasForeignKey(cp => cp.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CustomerPayment>()
                .HasOne(cp => cp.SalesInvoice)
                .WithMany(si => si.Payments)
                .HasForeignKey(cp => cp.SalesInvoiceId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure SalesType relationships
            builder.Entity<SalesInvoice>()
                .HasOne(si => si.SalesType)
                .WithMany(st => st.SalesInvoices)
                .HasForeignKey(si => si.SalesTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure VehicleType relationships
            builder.Entity<CustomerVehicle>()
                .HasOne(cv => cv.VehicleType)
                .WithMany(vt => vt.CustomerVehicles)
                .HasForeignKey(cv => cv.VehicleTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure InsurancePlan relationships
            builder.Entity<InsurancePlan>()
                .HasOne(ip => ip.InsuranceClient)
                .WithMany(ic => ic.InsurancePlans)
                .HasForeignKey(ip => ip.InsuranceClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure LineOfBusiness relationships
            builder.Entity<LineOfBusiness>()
                .HasOne(lob => lob.InsuranceClient)
                .WithMany(ic => ic.LinesOfBusiness)
                .HasForeignKey(lob => lob.InsuranceClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}