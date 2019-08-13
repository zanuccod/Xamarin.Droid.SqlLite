using SqLiteEntityFramework.Entities;
using SqLiteEntityFramework.Models;

namespace SqLiteEntityFramework
{
    public class App
    {
        public static void Initialize()
        {
            ServiceLocator.Instance.Register<IDataStore<Author>, AuthorDataStore>();

        }
    }
}
