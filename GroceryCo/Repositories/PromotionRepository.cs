using GroceryCo.Models;
using System.Collections.Generic;
using System.Linq;

namespace GroceryCo.Repositories
{
    /// <summary>
    /// Repository for accessing promotions catalog
    /// </summary>
    public class PromotionRepository : IPromotionRepository
    {
        private IFakeDataContext _context;

        public PromotionRepository(IFakeDataContext context)
        {
            _context = context;
        }

        public Promotion GetPromotion(int id)
        {
            return _context.Promotions.SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Promotion> GetPromotions()
        {
            return _context.Promotions;
        }
    }
}
