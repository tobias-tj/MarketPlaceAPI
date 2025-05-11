namespace Application.Wrappers
{
    public class ResponseData<T>
    {
        public T? Items { get; set; }
        public int TotalRegistros { get; set; }

        public static implicit operator ResponseData<T>(string v)
        {
            throw new NotImplementedException();
        }
    }
}
