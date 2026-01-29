using MauiAppDOUParser.ViewModels;

namespace MauiAppDOUParser
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ArticlesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}