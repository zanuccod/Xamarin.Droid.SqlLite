using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using EF.Common.Entities;
using EF.Common.Models;
using EF.Common.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace EF.Common.Test.ViewModels
{
    [TestFixture]
    public class ItemsViewModelTests
    {
        Mock<IDataStore<Author>> mockAuthorModel;
        private IDataStore<Author> authorModel;
        private ItemsViewModel viewModel;

        [SetUp]
        public void Setup()
        {
            mockAuthorModel = new Mock<IDataStore<Author>>();
            authorModel = mockAuthorModel.Object;

            viewModel = new ItemsViewModel(authorModel);
        }

        [Test]
        public void Constructor_NotNullElements_Success()
        {
            viewModel = new ItemsViewModel();
            Assert.NotNull(viewModel.Items);
            Assert.AreEqual(0, viewModel.Items.Count);
            Assert.NotNull(viewModel.AddItemCommand);
            Assert.NotNull(viewModel.LoadItemsCommand);
            Assert.False(viewModel.IsBusy);
        }

        [Test]
        public void LoadItemsCommand_ReadTwoElements_Success()
        {
            // Arrange
            var expectedResult = new List<Author>
            {
                new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "USA" },
                new Author() { Name = "name_2", Surname = "surname_2", BornDate = "02-02-1972", Country = "JAPAN" }
            };

            // mock expected result of GetItemsAsync() method
            mockAuthorModel.Setup(x => x.GetItemsAsync()).Returns(Task.FromResult(expectedResult));

            // Act
            viewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(expectedResult, viewModel.Items);
            Assert.False(viewModel.IsBusy);
        }

        [Test]
        public void AddItemAsync_AddItemThenRead_Success()
        {
            // Arrange
            DbContextOptions<EntityFrameworkBase<Author>> options = new DbContextOptionsBuilder<EntityFrameworkBase<Author>>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // create dataStore with "memory" connection option to test Add and then read operations
            authorModel = new AuthorDataStore(options);
            viewModel = new ItemsViewModel(authorModel);

            var expectedResult = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "USA" };

            // Act
            viewModel.AddItemCommand.Execute(expectedResult);
            viewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.True(expectedResult.Equals(viewModel.Items.First()));
            Assert.False(viewModel.IsBusy);
        }
    }
}
