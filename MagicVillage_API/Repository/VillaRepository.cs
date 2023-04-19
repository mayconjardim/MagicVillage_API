using MagicVillage_API.Data;
using MagicVillage_API.Model;
using MagicVillage_API.Repository.IRepository;


namespace MagicVillage_API.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly DataContext _context;

        public VillaRepository(DataContext context): base(context) 
        {
            _context = context;
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _context.Villas.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
