using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using SQLite;
using SqLitePcl.Common.Entities;

namespace SqLitePcl.Common.Models
{
    public class AuthorDataStoreSql : IDataStore<Author>
    {
        private readonly string dbPath;
        public AuthorDataStoreSql(string dbPath = null)
        {
            this.dbPath = dbPath;

            using (var conn = new SqLiteBase(dbPath))
            {
                // create table if not exist
                conn.db.CreateTableAsync<Author>();
            }
        }

        public async Task AddItemAsync(Author item)
        {
            using (var conn = new SqLiteBase(dbPath))
            {
                await conn.db.InsertAsync(item).ConfigureAwait(false);
            }
        }

        public async Task UpdateItemAsync(Author item)
        {
            using (var conn = new SqLiteBase(dbPath))
            {
                await conn.db.UpdateAsync(item).ConfigureAwait(false);
            }
        }

        public async Task DeleteItemAsync(Author item)
        {
            using (var conn = new SqLiteBase(dbPath))
            {
                await conn.db.DeleteAsync(item).ConfigureAwait(false);
            }
        }

        public async Task<Author> GetItemAsync(long item)
        {
            using (var conn = new SqLiteBase(dbPath))
            {
                return await conn.db.Table<Author>().FirstOrDefaultAsync(x => x.Id == item).ConfigureAwait(false);
            }
        }

        public async Task<List<Author>> GetItemsAsync()
        {
            using (var conn = new SqLiteBase(dbPath))
            {
                return await conn.db.Table<Author>().OrderByDescending(x => x.Id).ToListAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteAllAsync()
        {
            using (var conn = new SqLiteBase(dbPath))
            {
                await conn.db.DeleteAllAsync<Author>().ConfigureAwait(false);
            }
        }
    }
}
