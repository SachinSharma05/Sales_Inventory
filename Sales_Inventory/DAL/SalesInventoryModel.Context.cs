

//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Sales_Inventory.DAL
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class Sales_InventoryEntities : DbContext
{
    public Sales_InventoryEntities()
        : base("name=Sales_InventoryEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<AddNewCredit> AddNewCredits { get; set; }

    public DbSet<Advance> Advances { get; set; }

    public DbSet<MiscExpens> MiscExpenses { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<PaymentReceipt> PaymentReceipts { get; set; }

    public DbSet<ProductType> ProductTypes { get; set; }

    public DbSet<Purchase> Purchases { get; set; }

    public DbSet<Purchase_Product> Purchase_Product { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Sale> Sales { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<InHouse> InHouses { get; set; }

    public DbSet<InHouseVoucher> InHouseVouchers { get; set; }

    public DbSet<Sale_Product> Sale_Product { get; set; }

}

}

