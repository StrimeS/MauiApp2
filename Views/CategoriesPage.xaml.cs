using MauiApp1.Models;
using MauiApp1.ViewModels;

namespace MauiApp1.Views
{

    public partial class CategoriesPage : ContentPage
    {
        CategoriesViewModel _viewModel;
  
        public CategoriesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CategoriesViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
       
    }
}
