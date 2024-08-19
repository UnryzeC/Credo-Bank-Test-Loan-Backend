using LoanService.Core.Loan;
using LoanService.Infrastructure.Persistance.Configurations.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanService.Infrastructure.Persistance.Configurations.Loan;

internal class LoanRequestConfig : BaseEntityConfig<LoanRequest>
{
    public override void Configure( EntityTypeBuilder<LoanRequest> builder )
    {
        base.Configure( builder );

        builder.ToTable( "loan_requests" );

        builder.HasKey( lr => lr.Id );

        builder.Property( lr => lr.Id ).IsRequired( ).ValueGeneratedOnAdd( );

        builder.Property( lr => lr.Type ).IsRequired( );

        builder.Property( lr => lr.RequestedAmount ).IsRequired( );

        builder.Property( lr => lr.Currency ).IsRequired( );

        builder.Property( lr => lr.LoanPeriod ).IsRequired( );

        builder.Property( lr => lr.Status ).IsRequired( );

        builder.HasOne( lr => lr.User ).WithMany( u => u.LoanRequests ).HasForeignKey( lr => lr.UserId ).OnDelete( DeleteBehavior.NoAction );
    }
}
