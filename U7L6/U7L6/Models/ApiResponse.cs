namespace U7L6.Models
{
    public class ApiResponse<T>
    {
        public bool succeeded { get; set; }
        public int errorCode { get; set; }
        public string errorMessage { get; set; }
        public T data { get; set; }
    }

}
