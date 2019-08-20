using Android.App;
using Android.Widget;
using Android.OS;
using EntityFramework.Droid.Adapters;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using Android.Content;
using Android.Support.Design.Widget;
using Common.ViewModels;
using EF.Common.Entities;

namespace EntityFramework.Droid.Activities
{
    [Activity(Label = "Xamarin.Droid.SqlLite", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private ItemsAdapter adapter;
        private FloatingActionButton addBtn;

        private ItemsViewModel<Author> viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            viewModel = new ItemsViewModel<Author>();

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;
            recyclerView.SetAdapter(adapter = new ItemsAdapter(this, viewModel));

            addBtn = FindViewById<FloatingActionButton>(Resource.Id.add_btn);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            addBtn.Click += AddBtn_Click;
        }

        private void AddBtn_Click(object sender, System.EventArgs e)
        {
            var item = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" };
            viewModel.AddItemCommand.Execute(item);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            addBtn.Click -= AddBtn_Click;

            addBtn.Dispose();
            adapter.Dispose();
        }
    }
}

