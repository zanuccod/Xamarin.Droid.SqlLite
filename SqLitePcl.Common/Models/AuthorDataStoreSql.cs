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
        { }

        public async Task AddItemAsync(Author item)
        {
            await db.InsertAsync(item);
        }

        public async Task UpdateItemAsync(Author item)
        {
            await db.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(Author item)
        {
            await db.DeleteAsync(item);
        }

        public async Task<Author> GetItemAsync(long item)
        {
            return await db.Table<Author>().FirstOrDefaultAsync(x => x.Id == item);
        }

        public async Task<List<Author>> GetItemsAsync()
        {
            return await db.Table<Author>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task DeleteAllAsync()
        {
            await db.DeleteAllAsync<Author>();
        }
    }
}
