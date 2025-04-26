namespace Rased.Business.Dtos.Auths
{
    public class AuthResponseDto
    {
        public bool HasEmailError { get; set; }
        public bool IsBanned { get; set; }
        public string? AccountStatus { get; set; }
        public string? AccessToken { get; set; }
    }
}
