using LoanService.Core.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanService.Infrastructure.Persistance.Configurations.Common;

internal abstract class BaseEntityConfig<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure( EntityTypeBuilder<TEntity> builder )
    {
        builder.HasQueryFilter( e => !e.Deleted );

        builder.Property( x => x.CreatedById ).IsRequired( );

        builder.Property( x => x.CreatedDateTime ).HasDefaultValueSql( "CURRENT_TIMESTAMP" ).ValueGeneratedOnAdd( );
    }
}
