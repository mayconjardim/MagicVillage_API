using MagicVillage_API.Data;
using MagicVillage_API.Model;
using MagicVillage_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVillage_API.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly DataContext _context;

        public VillaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Villa> Create(Villa villa)
        {
           _context.Villas.Add(villa);
           await Save();
           return villa;
        }

        public async Task<Villa> Get(Expression<Func<Villa,bool>> filter = null, bool tracked = true)
        {

            IQueryable<Villa> query = _context.Villas;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null)
        {
            IQueryable<Villa> query = _context.Villas;

            if (filter != null)
            {
                query = query.Where(filter); 
            }

            return await query.ToListAsync();
        }

        public async Task Remove(Villa villa)
        {
            _context.Villas.Add(villa);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
