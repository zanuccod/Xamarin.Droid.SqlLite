using System;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using EntityFramework.Entities;
using EntityFramework.ViewModels;

namespace EntityFramework.Droid.Adapters
{
    public class ItemsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<Author> ItemClick;
        public event EventHandler<Author> ItemLongClick;

        private readonly Activity activity;
        private readonly ItemsViewModel viewModel;


        public ItemsAdapter(Activity activity, ItemsViewModel viewModel)
        {
            this.activity = activity;
            this.viewModel = viewModel;

            // update UI if collection changes
            this.viewModel.Items.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        private void OnClick(int position) => ItemClick?.Invoke(this, viewModel.Items[position]);
        private void OnLongClick(int position) => ItemLongClick?.Invoke(this, viewModel.Items[position]);

        public override int ItemCount => viewModel.Items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            throw new NotImplementedException();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }
    }
}
