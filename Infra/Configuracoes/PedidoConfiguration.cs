using DurableFunction.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DurableFunction.Infra.Configuracoes;

public class PedidoConfiguration
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired().HasColumnType("uniqueidentifier"); ;

        builder.Property(p => p.DataPedido)
            .IsRequired().HasColumnType("datetime");

        builder.Property(p => p.DataProcessamento)
            .IsRequired().HasColumnType("datetime");

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.ValorPedido)
            .IsRequired().HasColumnType("double");
    }
}
