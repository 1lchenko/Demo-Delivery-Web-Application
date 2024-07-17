namespace Demo_Delivery.Domain.SeedWork;

public interface IInicialData
{
    public Type EntityType { get; }
    public IEnumerable<object> GetData();
}