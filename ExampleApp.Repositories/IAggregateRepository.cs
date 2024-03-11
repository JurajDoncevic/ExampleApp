namespace ExampleApp.Repositories;

public interface IAggregateRepository<TKey, TDbModel>
{
    TDbModel? GetAggregate(TKey id);
    IEnumerable<TDbModel> GetAllAggregates();
    TDbModel? UpdateAggregate(TKey id, TDbModel entity);
}
