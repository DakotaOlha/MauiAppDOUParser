namespace MauiAppDOUParser.Models
{
    public class DouArticle
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
    }
}