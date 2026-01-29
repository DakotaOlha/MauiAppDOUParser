using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MauiAppDOUParser.Models;
using MauiAppDOUParser.Services;

namespace MauiAppDOUParser.ViewModels
{
    public partial class ArticlesViewModel : ObservableObject
    {
        private readonly IDouParserService _parserService;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private ObservableCollection<DouArticle> _articles = new();

        [ObservableProperty]
        private string _statusMessage = "Натисніть кнопку для завантаження статей";

        public ArticlesViewModel(IDouParserService parserService)
        {
            _parserService = parserService;
        }

        [RelayCommand]
        private async Task LoadArticlesAsync()
        {
            if (IsRefreshing)
                return;

            IsRefreshing = true;
            StatusMessage = "Завантаження статей з DOU...";

            try
            {
                var articles = await _parserService.GetArticlesAsync();

                Articles.Clear();
                foreach (var article in articles)
                {
                    Articles.Add(article);
                }

                StatusMessage = $"Завантажено {articles.Count} статей";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Помилка: {ex.Message}";
                await App.Current.MainPage.DisplayAlert("Помилка", ex.Message, "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task OpenLinkAsync(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    await Launcher.Default.OpenAsync(url);
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Помилка",
                        $"Не вдалося відкрити посилання: {ex.Message}", "OK");
                }
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadArticlesAsync();
        }
    }
}