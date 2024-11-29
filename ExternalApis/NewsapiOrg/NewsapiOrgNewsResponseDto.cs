namespace LatokenTask.ExternalApis.NewsapiOrg
{ 
    public class Article
    {
        public string Title { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }
    }

    public class NewsapiOrgNewsResponseDto
    {
        public List<Article> Articles { get; set; }
    }
}
