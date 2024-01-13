using ecommerce.Data.Base;
using ecommerce.Data.Services.Interfaces;
using ecommerce.Models;

namespace ecommerce.Data.Services.Classes
{
    public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(AppDbContext context) : base(context) { }
    }
}
