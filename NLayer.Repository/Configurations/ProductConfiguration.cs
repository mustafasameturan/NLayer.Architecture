using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;

namespace NLayer.Repository.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).UseIdentityColumn();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Stock).IsRequired();
        builder.Property(c => c.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.HasOne(c => c.Category).WithMany(c => c.Products).HasForeignKey(c => c.CategoryId);
        
        builder.ToTable("Products");
    }
}