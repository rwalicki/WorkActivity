namespace Work.Core.Interfaces
{
    public interface IFilterService<T>
    {
        bool Filter(T item, IConditionsService<T> conditions);
    }
}