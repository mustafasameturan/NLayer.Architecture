using System.Linq.Expressions;

namespace NLayer.Core.Services;

public interface IService<T> where T : class
{
    Task<T> GetByIdAsync(int id);

    Task<IEnumerable<T>> GetAllAsync();

    //İstediğimiz şekilde query yazalbildiğiniz için List yerine IQueryable kullandık 
    IQueryable<T> Where(Expression<Func<T, bool>> expression);

    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    
    //Servis tarafında veritabanına kaydederken saveChangesAsync kullanıldığı için Add-Update-Delete 
    //Metodlarını async olarak güncelledik
    Task AddAsync(T entity);
    
    Task AddRangeAsync(IEnumerable<T> entities);
    
    Task UpdateAsync(T entity);

    Task RemoveAsync(T entity);
    
    Task RemoveRangeAsync(IEnumerable<T> entities);
}