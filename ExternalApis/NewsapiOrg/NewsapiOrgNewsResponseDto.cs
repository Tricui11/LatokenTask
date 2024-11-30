namespace LatokenTask.ExternalApis.NewsapiOrg
{
    public class NewsapiOrgNewsResponseDto
    {
        public List<NewsapiOrgArticle> Articles { get; set; }
    }

    public class NewsapiOrgArticle
    {
        public string Title { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }
    }
}
