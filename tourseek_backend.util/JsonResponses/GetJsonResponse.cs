﻿namespace tourseek_backend.util.JsonResponses
{
    public class GetJsonResponse
    {
        public string StatusMessage { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
    }
}
