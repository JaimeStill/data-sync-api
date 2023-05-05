using Contracts.Process;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Process.Models;

namespace Process.Data.Config;
public class PackageConfig : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder
            .Property(x => x.State)
            .IsRequired()
            .HasDefaultValue(ProcessState.Pending)
            .HasConversion(
                x => x.ToString(),
                x => (ProcessState)Enum.Parse(typeof(ProcessState), x)
            );
    }
}