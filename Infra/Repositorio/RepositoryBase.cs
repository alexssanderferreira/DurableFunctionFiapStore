using DurableFunction.Infra;
using DurableFunction.Infra.Contratos;
using DurableFunction.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infra.Repositorio;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Base
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task AdicionarAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AlterarAsync(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<TEntity> ObterPorIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }
}
