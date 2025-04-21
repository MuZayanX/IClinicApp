namespace IClinicApp.API.Models.Helpers
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public bool Status { get; set; }
        public int Code { get; set; }
    }
}
