using Microsoft.Data.SqlClient.DataClassification;

namespace UNI_ASSETS.Data
{
    public interface IBaseRepository<T>
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();

        T GetById(int id);
        T GetById(string id);

    }
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected AppDbContext context;
        public BaseRepository(AppDbContext context)
        {
            this.context = context;       
        }
        public void Create(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public T GetById(string id)
        {
            return context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}
