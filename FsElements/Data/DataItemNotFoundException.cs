namespace FsElements.Data
{
    public class DataItemNotFoundException : Exception
    {
        public DataItemNotFoundException() : base("Item not found by Id") { }
    }
}
