using EntityFramework.Entities;
using EntityFramework.Models;

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
