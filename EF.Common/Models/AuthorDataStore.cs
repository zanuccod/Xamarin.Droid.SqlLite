using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EF.Common.Entities;
using Common.Models;

namespace EF.Common.Models
{
    public class AuthorDataStore : EFDataContext, IDataStore<Author>
    {
        private readonly DbContextOptions options;

        #region Constructors

        public AuthorDataStore()
        {
            options = new DbContextOptionsBuilder().Options;
        }

        public AuthorDataStore(DbContextOptions options)
        {
            this.options = options;
        }

        #endregion

        public async Task AddItemAsync(Author item)
        {
            using (var db = new EFDataContext(options))
            {
                await db.Authors.AddAsync(item);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateItemAsync(Author item)
        {
            using (var db = new EFDataContext(options))
            {
                db.Authors.Update(item);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteItemAsync(Author item)
        {
            using (var db = new EFDataContext(options))
            {
                db.Authors.Remove(item);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<Author> GetItemAsync(long id)
        {
            using (var db = new EFDataContext(options))
            {
                return await db.Authors.FindAsync(id);
            }
        }

        public async Task<List<Author>> GetItemsAsync()
        {
            using (var db = new EFDataContext(options))
            {
                return await db.Authors.OrderByDescending(x => x.Id).ToListAsync();
            }
        }

        public async Task DeleteAllAsync()
        {
            using (var db = new EFDataContext(options))
            {
                var items = await db.Authors.ToArrayAsync();
                db.Authors.RemoveRange(items);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
