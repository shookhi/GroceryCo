using System;

namespace GroceryCo.Repositories
{
    /// <summary>
    /// Unit of work for data access used by the app
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        //db context
        private IFakeDataContext _dbContext;

        //Repositories
        private IProductRepository _productRepository;
        private IPromotionRepository _promotionRepository;

        public UnitOfWork()
        {
            _dbContext = new FakeDataContext();
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ?? (_productRepository = new ProductRepository(_dbContext));
            }
        }

        public IPromotionRepository PromotionRepository
        {
            get
            {
                return _promotionRepository ?? (_promotionRepository = new PromotionRepository(_dbContext));
            }
        }

        public void Save()
        {
            //todo: db context save
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //todo: db context dispose
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
