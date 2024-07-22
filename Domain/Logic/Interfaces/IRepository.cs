namespace Domain.Logic.Intefaces;

public interface IRepository<T> where T : class
{
    public bool Create(T item);
    public IEnumerable<T> GetAll();
    public bool Update(T item);
    public bool Delete(int id);
    public void Save();
}