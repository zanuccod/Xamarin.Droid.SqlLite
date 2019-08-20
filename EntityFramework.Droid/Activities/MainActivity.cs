using Android.App;
using Android.Widget;
using Android.OS;
using EntityFramework.Droid.Adapters;
using EntityFramework.ViewModels;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using Android.Content;

namespace EntityFramework.Droid.Activities
{
    [Activity(Label = "Xamarin.Droid.SqlLite", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private ItemsAdapter adapter;
        private ItemsViewModel viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            viewModel = new ItemsViewModel();

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;
            recyclerView.SetAdapter(adapter = new ItemsAdapter(this, viewModel));
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            adapter.ItemClick += Adapter_ItemClick;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            adapter.ItemClick -= Adapter_ItemClick;
        }

        private void Adapter_ItemClick(object sender, Entities.Author e)
        {
            using (var intent = new Intent(this, typeof(ItemDetailActivity)))
            {
                intent.PutExtra("data", JsonConvert.SerializeObject(e));
                StartActivity(intent);
            }
        }
    }
}

