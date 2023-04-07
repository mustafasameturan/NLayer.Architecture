using System.Linq.Expressions;

namespace NLayer.Core.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);

    IQueryable<T> GetAll();

    //İstediğimiz şekilde query yazalbildiğiniz için List yerine IQueryable kullandık 
    IQueryable<T> Where(Expression<Func<T, bool>> expression);

    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    
    Task AddAsync(T entity);
    //Range metodları birden fazla veri ile işlem yapabilir
    Task AddRangeAsync(IEnumerable<T> entities);
    //Update ve delete işlemlerinde, hafızada sadece state'in durumu değiştiği için async değil
    //Uzun sürmüyor
    void Update(T entity);

    void Remove(T entity);

    //Range metodları birden fazla veri ile işlem yapabilir
    void RemoveRange(IEnumerable<T> entities);
}