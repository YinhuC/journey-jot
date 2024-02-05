using System;

namespace JourneyJot.Intefaces

{
    public interface IRepository<T>
    {
        T GetById(Guid id);

        IEnumerable<T> GetAll();

        bool Create(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        bool Exists(Guid id);

        bool Save();

    }
}

