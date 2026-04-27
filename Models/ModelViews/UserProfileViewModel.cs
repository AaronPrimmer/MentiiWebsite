namespace MentiiWebsite.Models.ModelViews
{
    public class UserProfileViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserTitle { get; set; } = string.Empty;
        public DateTime UserBirthday { get; set; }
        public List<string> UserSkills { get; set; } = [];
    }

}
