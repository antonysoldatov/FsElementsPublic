namespace FsElements.Models.Admin
{
    public class UserWithRolesViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public bool IsActiveSeller { get; set; }
    }
}
