using DurableFunction.Model;
using System;
using System.Threading.Tasks;

namespace DurableFunction.Infra.Contratos;

public interface IRepositoryBase<TEntity> where TEntity : Base
{
    Task<TEntity> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(TEntity entity);
    Task AlterarAsync(TEntity entity);
}
