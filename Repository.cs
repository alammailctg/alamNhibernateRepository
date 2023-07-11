using alamapp.Infrastructure.Domain;
using alamapp.Infrastructure.UnitOfWorks;
using alamapp.Repositories.NH.SessionStorage;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alamapp.Repositories.NH.PersistenceRepository
{
   public class Repository<T, TEntityKey> where T:IAggregateRoot
    {
        private IUnitOfWork _uow;
        public Repository(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public void Add(T entity)
        {
            SessionFactory.GetCurrentSession().Save(entity);
        }

        public void Save(T entity)
        {
            SessionFactory.GetCurrentSession().SaveOrUpdate(entity);
        }

        public void Delete(T entity)
        {
            SessionFactory.GetCurrentSession().Delete(entity);
        }

        public T FindBy(TEntityKey id)
        {
           return SessionFactory.GetCurrentSession().Get<T>(id);
        }

        public IEnumerable<T> FindAll()
        {
            ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
            return (List<T>)criteriaQuery.List<T>();
        }

        public IEnumerable<T> FindAll(int index, int count)
        {
            ICriteria criteriaQuery =
                      SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));

            return (List<T>)criteriaQuery.SetFetchSize(count)
                                    .SetFirstResult(index).List<T>();
        }

        public IEnumerable<T> FindBy(Infrastructure.Querying.Query query)
        {
            ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
            AppendCriteria(criteriaQuery);
            query.TranslateIntoNHQuery<T>(criteriaQuery);
            return criteriaQuery.List<T>();
        }

        public IEnumerable<T> FindBy(Infrastructure.Querying.Query query, int index, int count)
        {
            ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
            AppendCriteria(criteriaQuery);
            query.TranslateIntoNHQuery<T>(criteriaQuery);
            return criteriaQuery.SetFetchSize(count).SetFirstResult(index).List<T>();
        }
       public virtual void AppendCriteria(ICriteria criteria)
        {

        }

    }
}
