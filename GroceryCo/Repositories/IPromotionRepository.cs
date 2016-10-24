using GroceryCo.Models;
using System.Collections.Generic;

namespace GroceryCo.Repositories
{
    public interface IPromotionRepository
    {
        Promotion GetPromotion(int id);
        IEnumerable<Promotion> GetPromotions();
    }
}
