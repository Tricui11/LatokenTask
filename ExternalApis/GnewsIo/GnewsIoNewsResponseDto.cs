namespace LatokenTask.ExternalApis.GnewsIo
{
    public class GnewsIoNewsResponseDto
    {
        public List<GnewsIoArticle> Articles { get; set; }
    }

    public class GnewsIoArticle
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
