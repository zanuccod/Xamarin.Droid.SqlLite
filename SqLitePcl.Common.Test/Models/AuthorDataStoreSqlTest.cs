using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SqLitePcl.Common.Entities;
using SqLitePcl.Common.Models;

namespace SqLitePcl.Common.Test.Models
{
    [TestFixture]
    public class AuthorDataStoreSqlTest
    {
        private AuthorDataStoreSql db;

        [SetUp]
        public void SetUp()
        {
            db = new AuthorDataStoreSql("DataSource=:memory:");

            // because "DataSource=:memory:" create file in the executable folder to store database.
            // we delete every time all data to have always empty table for the tests
            Task.FromResult(db.DeleteAllAsync());
        }

        [Test]
        public void InsertOneElement_Succes()
        {
            var item = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" };
            Task.FromResult(db.AddItemAsync(item));

            Assert.AreEqual(1, db.GetItemsAsync().Result.Count);
        }

        [Test]
        public void UpdateElement_Succes()
        {
            var item = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" };
            Task.FromResult(db.AddItemAsync(item));

            // reload element to get item with given Id
            item = db.GetItemsAsync().Result.FirstOrDefault();
            item.Name = "name1";
            Task.FromResult(db.UpdateItemAsync(item));

            Assert.True(item.Equals(db.GetItemsAsync().Result.FirstOrDefault()));
        }

        [Test]
        public void DeleteElement_Success()
        {
            var item = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" };
            Task.FromResult(db.AddItemAsync(item));

            // reload element to get item with given Id
            item = db.GetItemsAsync().Result.FirstOrDefault();
            Task.FromResult(db.DeleteItemAsync(item));

            Assert.AreEqual(0, db.GetItemsAsync().Result.Count);
        }
    }
}
