using Microsoft.EntityFrameworkCore;

namespace SeuApp.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _ctx;
    private readonly DbSet<T> _set;

    public GenericRepository(AppDbContext ctx)
    {
        _ctx = ctx;
        _set = _ctx.Set<T>();
    }

    public Task<T?> GetByIdAsync(int id) => _set.FindAsync(id).AsTask();
    public Task<List<T>> GetAllAsync() => _set.ToListAsync();
    public Task<List<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        => _set.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity) { await _set.AddAsync(entity); }
    public Task UpdateAsync(T entity) { _set.Update(entity); return Task.CompletedTask; }
    public Task DeleteAsync(T entity) { _set.Remove(entity); return Task.CompletedTask; }
    public Task SaveChangesAsync() => _ctx.SaveChangesAsync();
}
