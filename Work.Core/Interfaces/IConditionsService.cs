namespace Work.Core.Interfaces
{
    public interface IConditionsService<T>
    {
        bool IsSatisfied(T item);
    }
}