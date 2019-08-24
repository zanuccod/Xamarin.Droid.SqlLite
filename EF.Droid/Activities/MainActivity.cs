using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Common.ViewModels;
using EF.Common.Entities;
using EF.Common.Models;
using EF.Droid.Adapters;

namespace EF.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ItemsAdapter adapter;
        private FloatingActionButton addBtn;

        private AuthorDataStore model;
        private ItemsViewModel<Author> viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            model = new AuthorDataStore();
            viewModel = new ItemsViewModel<Author>(model);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;
            recyclerView.SetAdapter(adapter = new ItemsAdapter(this, viewModel));

            addBtn = FindViewById<FloatingActionButton>(Resource.Id.fab);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            addBtn.Click += AddBtn_Click;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            addBtn.Click -= AddBtn_Click;

            model.Dispose();
            addBtn.Dispose();
            adapter.Dispose();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_delete_values)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void AddBtn_Click(object sender, EventArgs eventArgs)
        {
            var item = new Author() { Name = "name", Surname = "surname", BornDate = "01-01-1970", Country = "TEST" };
            viewModel.AddItemCommand.Execute(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

