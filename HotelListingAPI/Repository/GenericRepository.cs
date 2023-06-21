using System.Linq.Expressions;
using HotelListingAPI.Data;
using HotelListingAPI.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using X.PagedList;

namespace HotelListingAPI.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly Context _context;
    private readonly DbSet<T> _db;

    public GenericRepository(Context context)
    {
        _context = context;
        _db = _context.Set<T>();
    }

    public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
    {
        var query = _db.AsQueryable();

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<IPagedList<T>> GetPagedList(RequestParams requestParams, List<string> includes = null)
    {
        var query = _db.AsQueryable();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        return await query.AsNoTracking().ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);

    }


    public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
    {
        var query = _db.AsQueryable();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        return await query.AsNoTracking().FirstOrDefaultAsync(expression);
    }

    public async Task Insert(T entity)
    {
        await _db.AddAsync(entity);
    }

    public async Task InsertRange(IEnumerable<T> entities)
    {
        await _db.AddRangeAsync(entities);
    }

    public async Task Delete(int id)
    {
        var entity = await _db.FindAsync(id);
        _db.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _db.RemoveRange(entities);
    }

    public void Update(T entity)
    {
        _db.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
}