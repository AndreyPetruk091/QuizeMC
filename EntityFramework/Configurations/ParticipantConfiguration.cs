using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ValueObjects;

namespace EntityFramework.Configurations
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Paticipiant>
    {
        public void Configure(EntityTypeBuilder<Paticipiant> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Username)
                .IsRequired()
                .HasConversion(username => username.Value, str => new Username(str))
                .HasMaxLength(50);
        }
    }
}