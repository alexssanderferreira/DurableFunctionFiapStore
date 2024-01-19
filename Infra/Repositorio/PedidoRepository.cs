using DurableFunction.Infra.Contratos;
using DurableFunction.Model;
using Infra.Repositorio;

namespace DurableFunction.Infra.Repositorio;

public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
{
    public PedidoRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
