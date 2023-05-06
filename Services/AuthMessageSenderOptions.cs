﻿namespace jenjennewborncare.Services
{
    public class AuthMessageSenderOptions
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }
}