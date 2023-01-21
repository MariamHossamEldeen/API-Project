using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Repositories
{
    public interface IUnitOfWork : IDisposable 
    {
        //IGenericRepository<DeliveryMethod> deliveryMethodsRepo();
        //IGenericRepository<Product> productRepo();
        //IGenericRepository<ProductBrand> productBrandRepo();
        //IGenericRepository<ProductType> productTypeRepo();
        //IGenericRepository<Order> OrdersRepo();
        //IGenericRepository<OrderItem> orderItemsRepo();

        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        Task<int> Complete();
    }
}
