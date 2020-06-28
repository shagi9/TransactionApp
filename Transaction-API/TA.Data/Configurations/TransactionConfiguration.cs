using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TA.Data.Entities;

namespace TA.Data.Configurations
{
    class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(i => i.Amount)
                   .HasColumnType("Money");

            builder.HasData(
                new Transaction
                {
                    Id = 1,
                    Status = "Pending",
                    Type = "Refill",
                    ClientName = "Ilya Shagoferov",
                    Amount = 100.12m
                },
                new Transaction
                {
                    Id = 2,
                    Status = "Completed",
                    Type = "Withdrawal",
                    ClientName = "Vanya Efimov",
                    Amount = 200.20m
                },
                new Transaction
                {
                    Id = 3,
                    Status = "Cancelled",
                    Type = "Refill",
                    ClientName = "Roma Nagalevskiy",
                    Amount = 300.50m
                }
           );
        }
    }
}
