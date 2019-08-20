using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using SqLitePcl.Common.Entities;

namespace EF.Common.Models
{
    public class AuthorDataStore : IDataStore<Author>
    {
        Task IDataStore<Author>.AddItemAsync(Author item)
        {
            throw new System.NotImplementedException();
        }

        Task IDataStore<Author>.DeleteItemAsync(Author item)
        {
            throw new System.NotImplementedException();
        }

        Task<Author> IDataStore<Author>.GetItemAsync(long item)
        {
            throw new System.NotImplementedException();
        }

        Task<List<Author>> IDataStore<Author>.GetItemsAsync()
        {
            throw new System.NotImplementedException();
        }

        Task IDataStore<Author>.UpdateItemAsync(Author item)
        {
            throw new System.NotImplementedException();
        }
    }
}
