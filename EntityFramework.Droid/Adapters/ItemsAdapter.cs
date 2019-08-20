using System;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using EF.Common.Entities;
using EF.Common.ViewModels;

namespace EntityFramework.Droid.Adapters
{
    public class ItemsViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name { get; private set; }
        public TextView Country { get; private set; }

        public ItemsViewHolder(View itemView, Action<int> onClickListener, Action<int> onLongClickListener)
            : base(itemView)
        {
            // Locate and cache view references:
            Name = itemView.FindViewById<TextView>(Resource.Id.adapter_item_name);
            Country = itemView.FindViewById<TextView>(Resource.Id.adapter_item_country);

            itemView.Click += (sender, e) => onClickListener(AdapterPosition);
            itemView.LongClick += (sender, e) => onLongClickListener(AdapterPosition);
        }
    }

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
            ItemsViewHolder vh = holder as ItemsViewHolder;

            // Load the photo caption from the photo album:
            vh.Name.Text = string.Format("{0} {1}", viewModel.Items[position].Name, viewModel.Items[position].Surname);
            vh.Country.Text = viewModel.Items[position].Country;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                                          Inflate(Resource.Layout.adapter_item, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            ItemsViewHolder item = new ItemsViewHolder(itemView, OnClick, OnLongClick);
            return item;
        }
    }
}
