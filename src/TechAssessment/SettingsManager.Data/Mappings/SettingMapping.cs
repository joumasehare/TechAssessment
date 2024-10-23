using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SettingsManager.Domain.Models.Settings;

namespace SettingsManager.Data.Mappings;

internal class SettingMapping : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd().HasColumnName("SettingId"); 
    }
}