namespace Domain.Logic.Intefaces;

public interface IRepository<T> where T : class
{
    public bool Create(T item);
    IEnumerable<T> GetAll();
    public bool Update(T item);
    public bool Delete(int id);
    void Save();
}