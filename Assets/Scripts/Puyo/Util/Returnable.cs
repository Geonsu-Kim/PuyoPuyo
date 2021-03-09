public class Returnable<T>
{
    public T value { get; set; }
    public Returnable(T value)
    {
        this.value = value;
    }
}