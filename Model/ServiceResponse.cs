﻿namespace WebAPI6_Demo.Model
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
