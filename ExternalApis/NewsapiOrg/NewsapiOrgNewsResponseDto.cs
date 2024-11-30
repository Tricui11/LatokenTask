namespace LatokenTask.ExternalApis.NewsapiOrg
{ 
    public class NewsapiOrgArticleDto
    {
        public string Title { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }
    }

    public class NewsapiOrgNewsResponseDto
    {
        public List<NewsapiOrgArticleDto> Articles { get; set; }
    }
}
