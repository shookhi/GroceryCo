namespace GroceryCo.Repositories
{
    /// <summary>
    /// Interface for unit of work
    /// </summary>
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }

        IPromotionRepository PromotionRepository { get; }

        void Save();
    }
}
