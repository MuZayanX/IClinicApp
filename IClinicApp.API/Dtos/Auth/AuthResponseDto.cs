namespace IClinicApp.API.Dtos.Auth
{
    public class AuthResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public bool Status { get; set; }
        public int Code { get; set; }
        public AuthTokenDto? Data { get; set; } // Contains token and username
        public List<string>? Errors { get; set; } // Optional, for error details

    }
}
