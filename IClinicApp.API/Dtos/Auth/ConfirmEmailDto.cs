﻿namespace IClinicApp.API.Dtos.Auth
{
    public class ConfirmEmailDto
    {

        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
