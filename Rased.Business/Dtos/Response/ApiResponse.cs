using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Response
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; } = new();
        public string Message { get; set; } 

        public T Data { get; set; }

        // Take Data If Successed
        public ApiResponse(T data)
        {
            Succeeded = true;
            Data = data;
        }
        // Message If Success
        public ApiResponse(T data, string message)
        {
            Succeeded = true;
            Data = data;
            Message = message;
        }

        // Take List Of Errors If Not Successed
        public ApiResponse(List<string> errors)
        {
            Succeeded = false;
            Errors = errors;
        }

        // Take One Error If Not Successed
        public ApiResponse(string error)
        {
            Succeeded = false;
            Errors = new List<string> { error };
        }
    }

}
