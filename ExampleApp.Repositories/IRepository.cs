namespace ExampleApp.Repositories;

public interface IRepository<TKey, TDbModel>
{
    TDbModel? Get(TKey id);
    IEnumerable<TDbModel> Get(params TKey[] ids);
    IEnumerable<TDbModel> GetAll();
    TDbModel? Insert(TDbModel entity);
    TDbModel? Update(TKey id, TDbModel entity);
    bool Delete(TKey id);
}
