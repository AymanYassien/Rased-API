namespace Rased.Business.Dtos.Auths
{
    public class ReadUserDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Badge { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public byte[]? ProfilePic { get; set; }
    }
}
