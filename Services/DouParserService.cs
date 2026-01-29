using HtmlAgilityPack;
using MauiAppDOUParser.Models;

namespace MauiAppDOUParser.Services
{
    public class DouParserService : IDouParserService
    {
        private const string DOU_URL = "https://dou.ua/lenta/";

        public async Task<List<DouArticle>> GetArticlesAsync()
        {
            var articles = new List<DouArticle>();

            try
            {
                var web = new HtmlWeb();
                web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";

                var doc = await web.LoadFromWebAsync(DOU_URL);

                var articleNodes = doc.DocumentNode.SelectNodes("//article[contains(@class, 'post')]");

                if (articleNodes != null)
                {
                    foreach (var node in articleNodes)
                    {
                        try
                        {
                            var titleNode = node.SelectSingleNode(".//h2/a");
                            var dateNode = node.SelectSingleNode(".//div[@class='info']//span[@class='date']");
                            var authorNode = node.SelectSingleNode(".//div[@class='info']//a[@rel='author']");
                            var descNode = node.SelectSingleNode(".//p");
                            var viewsNode = node.SelectSingleNode(".//div[@class='info']//span[@class='views']");
                            var commentsNode = node.SelectSingleNode(".//div[@class='info']//a[contains(@href, '#comments')]");

                            if (titleNode != null)
                            {
                                var article = new DouArticle
                                {
                                    Title = HtmlEntity.DeEntitize(titleNode.InnerText).Trim(),
                                    Link = titleNode.GetAttributeValue("href", ""),
                                    Date = dateNode?.InnerText.Trim() ?? "Невідома дата",
                                    Author = authorNode?.InnerText.Trim() ?? "Невідомий автор",
                                    Description = descNode?.InnerText.Trim() ?? "",
                                    ViewsCount = ParseNumber(viewsNode?.InnerText),
                                    CommentsCount = ParseNumber(commentsNode?.InnerText)
                                };

                                if (!article.Link.StartsWith("http"))
                                {
                                    article.Link = "https://dou.ua" + article.Link;
                                }

                                articles.Add(article);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Помилка парсингу статті: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Помилка завантаження сторінки: {ex.Message}");
                throw new Exception("Не вдалося завантажити статті з DOU", ex);
            }

            return articles;
        }

        private int ParseNumber(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            var numbers = new string(text.Where(char.IsDigit).ToArray());

            return int.TryParse(numbers, out int result) ? result : 0;
        }
    }
}