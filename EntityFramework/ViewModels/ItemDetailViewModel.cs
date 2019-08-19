using System;
using EntityFramework.Entities;

namespace EntityFramework
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Author Item { get; set; }
        public ItemDetailViewModel(Author item = null)
        {
            if (item != null)
            {
                Title = item.Name;
                Item = item;
            }
        }
    }
}
