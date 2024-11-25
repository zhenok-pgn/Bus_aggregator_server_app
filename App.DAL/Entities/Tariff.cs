using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Entities
{
    public enum Currency
    {
        USD,
        EUR,
        RUB,
        KZT
    }

    public class Tariff
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Currency Currency {  get; set; }
        public List<Price> Prices { get; set; } = new();
    }

    public class TariffConfiguration : IEntityTypeConfiguration<Tariff>
    {
        public void Configure(EntityTypeBuilder<Tariff> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();
        }
    }
}
