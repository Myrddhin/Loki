namespace Loki.Common
{
    public interface IListNode<out T>
    {
        T Value { get; }
    }
}