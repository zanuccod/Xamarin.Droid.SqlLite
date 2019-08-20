using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Common.Helpers;
using Common.Models;
using Common.ViewModels;

namespace Common.ViewModels
{
    public class ItemsViewModel<T> : BaseViewModel
    {
        private readonly IDataStore<T> authorDataStore;

        public ObservableCollection<T> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command AddItemCommand { get; set; }

        /// <summary>
        /// pass data store as agrument to allow unit test with custom model
        /// </summary>
        /// <param name="model"></param>
        public ItemsViewModel(IDataStore<T> model = null)
        {
            authorDataStore = model ?? ServiceLocator.Instance.Get<IDataStore<T>>();
            Init();
        }

        private void Init()
        {
            Items = new ObservableCollection<T>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command<T>(async (T item) => await AddItem(item));
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

        async Task AddItem(T item)
        {
            Items.Add(item);
            await authorDataStore.AddItemAsync(item);
        }
    }
}
