using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class CategoryPage : ContentPage
{
	private CategoryViewModel _categoryViewModel;
	public CategoryPage()
	{
		InitializeComponent();
		BindingContext = _categoryViewModel = new CategoryViewModel();
	}
}