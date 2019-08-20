using Common.Helpers;
using Common.Models;
using EF.Common.Entities;
using EF.Common.Models;

namespace EntityFramework
{
    public class App
    {
        public static void Initialize()
        {
            ServiceLocator.Instance.Register<IDataStore<Author>, AuthorDataStore>();
        }
    }
}
