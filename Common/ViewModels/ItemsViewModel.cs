using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Common.Helpers;
using Common.Models;
using Common.ViewModels;

namespace Common.ViewModels
{
    public class ItemsViewModel<T> : BaseViewModel, IDisposable
    {
        private readonly IDataStore<T> authorDataStore;

        public ObservableCollection<T> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command AddItemCommand { get; set; }
        public Command DeleteAllCommand { get; set; }

        public ItemsViewModel(IDataStore<T> model)
        {
            authorDataStore = model;
            Init();
        }

        public void Dispose()
        {
            authorDataStore.Dispose();
        }

        private void Init()
        {
            Items = new ObservableCollection<T>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command<T>(async (T item) => await AddItem(item));
            DeleteAllCommand = new Command<T>(async (T item) => await DeleteAllItems());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                (await authorDataStore.GetItemsAsync()).ForEach(x => Items.Add(x));
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
            await authorDataStore.AddItemAsync(item);
            Items.Add(item);
        }

        async Task DeleteAllItems()
        {
            await authorDataStore.DeleteAllAsync();
            Items.Clear();
        }
    }
}
