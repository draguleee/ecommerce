using ecommerce.Data.Base;
using ecommerce.Data.Services.Interfaces;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Data.Services.Classes
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        public ActorsService(AppDbContext context) : base(context) { }
    }
}
