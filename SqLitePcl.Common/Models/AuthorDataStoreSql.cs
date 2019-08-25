using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using SqLitePcl.Common.Entities;

namespace SqLitePcl.Common.Models
{
    public class AuthorDataStoreSql : SqLiteBase, IDataStore<Author>
    {
        public AuthorDataStoreSql(string dbPath = null)
            : base(dbPath)
        {
            // create table if not exist
            db.CreateTableAsync<Author>();
        }

        public async Task AddItemAsync(Author item)
        {
            await db.InsertAsync(item).ConfigureAwait(false);
        }

        public async Task UpdateItemAsync(Author item)
        {
            await db.UpdateAsync(item).ConfigureAwait(false);
        }

        public async Task DeleteItemAsync(Author item)
        {
            await db.DeleteAsync(item).ConfigureAwait(false);
        }

        public async Task<Author> GetItemAsync(long item)
        {
            return await db.Table<Author>().FirstOrDefaultAsync(x => x.Id == item).ConfigureAwait(false);
        }

        public async Task<List<Author>> GetItemsAsync()
        {
            return await db.Table<Author>().OrderByDescending(x => x.Id).ToListAsync().ConfigureAwait(false);
        }

        public async Task DeleteAllAsync()
        {
            await db.DeleteAllAsync<Author>().ConfigureAwait(false);
        }
    }
}
