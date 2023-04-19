using MagicVillage_API.Model;

namespace MagicVillage_API.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa villa);

    }
}
