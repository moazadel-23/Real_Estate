using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Real_Estate.DataAccess.EntityConfigurations
{
    public class CartEntityTypeConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(e => new { e.PropertyId, e.UserId });
        }
    }
}
