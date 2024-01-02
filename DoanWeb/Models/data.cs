namespace DoanWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class data : DbContext
    {
        public data()
            : base("name=data")
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Producers> Producers { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductTypes> ProductTypes { get; set; }
        public virtual DbSet<Sessions> Sessions { get; set; }
        public virtual DbSet<User_role> User_role { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>()
                .HasMany(e => e.ProductTypes)
                .WithOptional(e => e.Categories)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Comments>()
                .Property(e => e.proID)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetails>()
                .Property(e => e.orderID)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetails>()
                .Property(e => e.proID)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetails>()
                .Property(e => e.ordtsThanhTien)
                .IsUnicode(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.orderID)
                .IsUnicode(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.o_phone)
                .IsUnicode(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.orderDateTime)
                .IsUnicode(false);

            modelBuilder.Entity<Producers>()
                .Property(e => e.pdcPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Producers>()
                .Property(e => e.pdcEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Products>()
                .Property(e => e.proID)
                .IsUnicode(false);

            modelBuilder.Entity<Products>()
                .Property(e => e.proSize)
                .IsUnicode(false);

            modelBuilder.Entity<Products>()
                .Property(e => e.proPrice)
                .IsUnicode(false);

            modelBuilder.Entity<Products>()
                .Property(e => e.proUpdateDate)
                .IsUnicode(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductTypes>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.ProductTypes)
                .WillCascadeOnDelete();
        }
    }
}
