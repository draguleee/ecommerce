using ecommerce.Data.Base;
using ecommerce.Data.Services.Interfaces;
using ecommerce.Models;

namespace ecommerce.Data.Services.Classes
{
    public class ProducersService : EntityBaseRepository<Producer>, IProducersService
    {
        public ProducersService(AppDbContext context) : base(context) { }
    }
}
