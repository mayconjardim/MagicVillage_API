using MagicVillage_API.Model;
using System.Linq.Expressions;

namespace MagicVillage_API.Repository.IRepository
{
    public interface IVillaRepository
    {

        Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null);

        Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked=true);

        Task<Villa> CreateAsync(Villa villa);

        Task UpdateAsync(Villa villa);

        Task RemoveAsync(Villa villa);

        Task SaveAsync();



    }
}
