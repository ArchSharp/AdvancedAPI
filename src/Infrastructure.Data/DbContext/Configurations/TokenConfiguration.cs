using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShareLoanApp.Domain.Entities;

namespace Infrastructure.Data.DbContext.Configurations
{
    class TokenConfiguration: IEntityTypeConfiguration<Tokens>
    {
        public void Configure(EntityTypeBuilder<Tokens> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Token).IsRequired();
        }
    }
}
