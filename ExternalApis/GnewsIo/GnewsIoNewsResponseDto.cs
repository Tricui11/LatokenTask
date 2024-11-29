namespace LatokenTask.ExternalApis.GnewsIo
{
    public class GnewsIoNewsResponseDto
    {
        public List<Article> Articles { get; set; }
    }

    public class Article
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
