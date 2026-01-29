using MauiAppDOUParser.Models;

namespace MauiAppDOUParser.Services
{
    public interface IDouParserService
    {
        Task<List<DouArticle>> GetArticlesAsync();
    }
}