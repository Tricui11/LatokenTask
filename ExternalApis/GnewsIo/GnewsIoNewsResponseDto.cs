namespace LatokenTask.ExternalApis.GnewsIo
{
    public class GnewsIoNewsResponseDto
    {
        public List<GnewsIoArticleDto> Articles { get; set; }
    }

    public class GnewsIoArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
