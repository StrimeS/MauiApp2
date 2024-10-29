using System.Collections.ObjectModel;
using System.Diagnostics;
using MauiApp1.Models;
using MauiApp1.Views;


namespace MauiApp1.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }
        public Command MoveToTopCommand { protected set; get; }
        public Command MoveToBottomCommand { protected set; get; }
        public Command RemoveCommand { protected set; get; }

        public ItemsViewModel()
        {
            Title = "ItemsPage";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Item>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
            MoveToTopCommand = new Command(MoveToTop);
            MoveToBottomCommand = new Command(MoveToBottom);
            RemoveCommand = new Command(Remove);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStoreItems.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        private void MoveToTop(object itemObj)
        {
            Item item = itemObj as Item;
            if (item == null) return;
            int oldIndex = Items.IndexOf(item);
            if (oldIndex > 0)
                Items.Move(oldIndex, oldIndex - 1);
        }
        private void MoveToBottom(object itemObj)
        {
            Item item = itemObj as Item;
            if (item == null) return;
            int oldIndex = Items.IndexOf(item);
            if (oldIndex < Items.Count - 1)
                Items.Move(oldIndex, oldIndex + 1);
        }
        private void Remove(object itemObj)
        {
            Item item = itemObj as Item;
            if (item == null) return;

            DataStoreItems.DeleteItemAsync(item.Id);

            Items.Remove(item);
        }


    }
}