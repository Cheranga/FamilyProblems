namespace Lengaburu.Domain.Models
{
    public class Status
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }

    public class Status<T> : Status
    {
        public T Data { get; set; }
    }
}