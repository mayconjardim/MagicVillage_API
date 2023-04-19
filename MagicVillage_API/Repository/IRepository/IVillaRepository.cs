using MagicVillage_API.Model;
using System.Linq.Expressions;

namespace MagicVillage_API.Repository.IRepository
{
    public interface IVillaRepository
    {

        Task<List<Villa>> GetAll(Expression<Func<Villa, bool>> filter = null);

        Task<Villa> Get(Expression<Func<Villa, bool>> filter = null, bool tracked=true);

        Task<Villa> Create(Villa villa);

        Task Remove(Villa villa);

        Task Save();



    }
}
