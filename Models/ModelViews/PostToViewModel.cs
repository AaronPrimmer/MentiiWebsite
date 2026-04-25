namespace MentiiWebsite.Models.ModelViews
{
    public class PostToViewModel
    {
        public Guid PostId { get; set; }

        public Guid UserUuid { get; set; }

        public string PostTitle { get; set; } = string.Empty;

        public string PostBody { get; set; } = string.Empty;

        public DateTime PostDate { get; set; }

        public string Username { get; set; } = string.Empty;

        public int LikeCount { get; set; }
    
        public bool UserHasLiked { get; set; }
    }
}
