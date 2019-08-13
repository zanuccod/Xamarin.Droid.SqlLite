using System;
using SqLiteEntityFramework.Entities;

namespace SqLiteEntityFramework
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
