﻿using System;
namespace BaseApi.Domain.Response
{
    public class ObjectResponse<T> where T:class
    {
        public T Object { get; set; }

        public bool Success { get; set; }
        public string Message { get; set; }

        private ObjectResponse(bool success, string message, T obj)
        {
            Success = success;
            Message = message;
            Object = obj;
        }

        //on success
        public ObjectResponse(T obj) : this(true, string.Empty, obj) { }

        //on error
        public ObjectResponse(string message) : this(false, message, null) { }
    }
}
