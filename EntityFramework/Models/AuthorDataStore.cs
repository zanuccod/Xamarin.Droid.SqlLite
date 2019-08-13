using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SqLiteEntityFramework.Entities;

namespace SqLiteEntityFramework.Models
{
    public class AuthorDataStore : EntityFrameworkBase<Author>, IDataStore<Author>
    {
        public AuthorDataStore()
        { }

        public AuthorDataStore(DbContextOptions<EntityFrameworkBase<Author>> options)
            : base(options)
        { }

        public async Task AddItemAsync(Author item)
        {
            using (var context = CreateContext())
            {
                await context.Table.AddAsync(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateItemAsync(Author item)
        {
            using (var context = CreateContext())
            {
                await Task.FromResult(context.Table.Update(item));
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteItemAsync(Author item)
        {
            using (var context = CreateContext())
            {
                await Task.FromResult(context.Table.Remove(item));
                await context.SaveChangesAsync();
            }
        }

        public async Task<Author> GetItemAsync(long id)
        {
            using (var context = CreateContext())
            {
                return await context.Table.FindAsync(id);
            }
        }

        public async Task<List<Author>> GetItemsAsync()
        {
            using (var context = CreateContext())
            {
                return await context.Table.OrderByDescending(x => x.Id)
                                .ToListAsync();
            }
        }
    }
}
