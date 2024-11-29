namespace LatokenTask.Models
{
    public class NewsArticle
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }

        public override string ToString()
        {
            return $"{PublishedAt:yyyy-MM-dd HH:mm} - {Title}\n{Content}";
        }
    }

}
