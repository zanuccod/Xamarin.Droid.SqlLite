using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EF.Common.Entities;
using Common.Models;
using System.Transactions;

namespace EF.Common.Models
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
            await Table.AddAsync(item).ConfigureAwait(false);
            await SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateItemAsync(Author item)
        {
            Table.Update(item);
            await SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteItemAsync(Author item)
        {
            Table.Remove(item);
            await SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Author> GetItemAsync(long id)
        {
            return await Table.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<List<Author>> GetItemsAsync()
        {
            return await Table.OrderByDescending(x => x.Id).ToListAsync().ConfigureAwait(false);
        }

        public async Task DeleteAllAsync()
        {
            var items = await Table.ToArrayAsync();
            Table.RemoveRange(items);
            await SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
