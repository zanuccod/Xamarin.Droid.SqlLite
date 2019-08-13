using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using SqLiteEntityFramework.Entities;
using SqLiteEntityFramework.Models;

namespace SqLiteEntityFramework.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private readonly IDataStore<Author> authorDataStore;

        public ObservableCollection<Author> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command AddItemCommand { get; set; }

        /// <summary>
        /// pass data store as agrument to allow unit test with custom model
        /// </summary>
        /// <param name="model"></param>
        public ItemsViewModel(IDataStore<Author> model = null)
        {
            authorDataStore = model ?? ServiceLocator.Instance.Get<IDataStore<Author>>();
            Init();
        }

        private void Init()
        {
            Items = new ObservableCollection<Author>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command<Author>(async (Author item) => await AddItem(item));
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await authorDataStore.GetItemsAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task AddItem(Author item)
        {
            Items.Add(item);
            await authorDataStore.AddItemAsync(item);
        }
    }
}
