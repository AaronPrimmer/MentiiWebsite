namespace MentiiWebsite.Models.ModelViews
{
    public class EditProfileViewModel
    {
        public string UserFirstName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserUuid { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime UserBirthday { get; set; }
        public string UserLastName { get; set; } = string.Empty;
        public string UserSkills { get; set; } = string.Empty;
        public string UserTitle { get; set; } = string.Empty;
    }
}
