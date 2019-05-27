using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestOnlineShared.Model;

namespace TestOnlineShared.Interface
{
    public interface IRepository<T>
    {
        #region Methods

        /// <summary>
        ///     Search all data from the specific table.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Search();

        /// <summary>
        /// Search data from specific table using specific condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IQueryable<T> Search(Expression<Func<T, bool>> condition);

        /// <summary>
        ///     Insert a record into data table.
        /// </summary>
        /// <param name="entity"></param>
        T Insert(T entity);

        /// <summary>
        ///     Remove a list of entities from database.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void Remove(IEnumerable<T> entities);

        /// <summary>
        ///     Remove an entity from database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Remove(T entity);

        void Insert(IEnumerable<T> lstEntity);

        void Update(T entityToUpdate);

        //void Update(IEnumerable<T> lstEntityToUpdate, IEnumerable<string> propertyNamesNotChanged = null);

        Task<bool> CheckExist(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null);

        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending,
            Ref<CheckError> checkError = null);

        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending, int skip,
            int take, Ref<CheckError> checkError = null);

        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string groupBy, string fieldOrderBy,
            bool ascending, int take, Ref<CheckError> checkError = null);

        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null);

        Task<int> GetCount(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null);

        Task<IEnumerable<T>> GetAll(Ref<CheckError> checkError = null);

        Task<T> GetById(object id, Ref<CheckError> checkError = null);

        Task<T> GetOne(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null);

        Task<IEnumerable<T>> Get(string storedProcedureName, SqlParameter[] parameters = null,
            Ref<CheckError> checkError = null);

        Task<T> GetOne(string storedProcedureName, SqlParameter[] parameters = null,
            Ref<CheckError> checkError = null);

        Task<IEnumerable<SqlParameter>> GetOutPut(string storedProcedureName, SqlParameter[] parameters,
            Ref<CheckError> checkError = null);

        Task<bool> Delete(object id, Ref<CheckError> checkError = null);
        Task<bool> Delete(T entity, Ref<CheckError> checkError = null);
        Task<bool> DeleteAll(IList<T> list, Ref<CheckError> checkError = null);

        #endregion
    }
}
