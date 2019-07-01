
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineShared.Interface;
using TestOnlineShared.Model;

namespace TestOnlineShared.Service
{
    public class Repository<T>:IRepository<T> where T :class
    {
        #region Properties

        /// <summary>
        ///     Database set.
        /// </summary>
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initiate repository with database context wrapper.
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Search all data from the specific table.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> Search()
        {
            return _dbSet;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Search(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Where(condition);
        }


        /// <summary>
        ///     Insert a record into data table.
        /// </summary>
        /// <param name="entity"></param>
        public virtual T Insert(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        /// <summary>
        ///     Remove a list of entities from database.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual void Remove(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        ///     Remove an entity from database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Insert a list entity
        /// </summary>
        /// <param name="lstEntity"></param>
        public virtual void Insert(IEnumerable<T> lstEntity)
        {
            _dbSet.AddRange(lstEntity);

        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }


        /// <summary>
        /// Check exist entity satisfy condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<bool> CheckExist(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dbContext.Set<T>().AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "CheckExist(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }

        /// <summary>
        /// Get a list of entity with order by
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="fieldOrderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool @ascending, Ref<CheckError> checkError = null)
        {
            try
            {
                var p = typeof(T).GetProperty(fieldOrderBy);
                var t = p.PropertyType;
                if (t == typeof(int))
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, int>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending
                        ? _dbContext.Set<T>().Where(predicate).OrderBy(expr1).ToListAsync()
                        : _dbContext.Set<T>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                }
                else
                {
                    if (t == typeof(int?))
                    {
                        var pe = Expression.Parameter(typeof(T), "p");
                        var expr1 = Expression.Lambda<Func<T, int?>>(Expression.Property(pe, fieldOrderBy), pe);
                        return await (ascending
                            ? _dbContext.Set<T>().Where(predicate).OrderBy(expr1).ToListAsync()
                            : _dbContext.Set<T>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                    }
                    else if (t == typeof(DateTime))
                    {
                        var pe = Expression.Parameter(typeof(T), "p");
                        var expr1 = Expression.Lambda<Func<T, DateTime>>(Expression.Property(pe, fieldOrderBy), pe);
                        return await (ascending ? _dbContext.Set<T>().Where(predicate).OrderBy(expr1).ToListAsync() : _dbContext.Set<T>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                    }
                    else
                    {
                        var pe = Expression.Parameter(typeof(T), "p");
                        var expr1 = Expression.Lambda<Func<T, String>>(Expression.Property(pe, fieldOrderBy), pe);
                        return await (ascending
                            ? _dbContext.Set<T>().Where(predicate).OrderBy(expr1).ToListAsync()
                            : _dbContext.Set<T>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                    }

                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get a list of entity with order by, paging 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="fieldOrderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool @ascending, int skip, int take, Ref<CheckError> checkError = null)
        {
            try
            {
                var p = typeof(T).GetProperty(fieldOrderBy);
                var t = p.PropertyType;
                if (t == typeof(int))
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, int>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending ? _dbContext.Set<T>().Where(predicate).OrderBy(expr1).Skip(skip).Take(take).ToListAsync() : _dbContext.Set<T>().Where(predicate).OrderByDescending(expr1).Skip(skip).Take(take).ToListAsync());
                }
                else if (t == typeof(DateTime))
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, DateTime>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending ? _dbContext.Set<T>().Where(predicate).OrderBy(expr1).Skip(skip).Take(take).ToListAsync() : _dbContext.Set<T>().Where(predicate).OrderByDescending(expr1).Skip(skip).Take(take).ToListAsync());
                }
                else
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, string>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending ? _dbContext.Set<T>().Where(predicate).OrderBy(expr1).Skip(skip).Take(take).ToListAsync() : _dbContext.Set<T>().Where(predicate).OrderByDescending(expr1).Skip(skip).Take(take).ToListAsync());
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending, int skip, int take, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// a list of entity with order by, group by, paging
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="groupBy"></param>
        /// <param name="fieldOrderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="take"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string groupBy, string fieldOrderBy, bool @ascending, int take, Ref<CheckError> checkError = null)
        {
            try
            {
                var p = typeof(T).GetProperty(fieldOrderBy);
                var t = p.PropertyType;
                if (t == typeof(int))
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, int>>(Expression.Property(pe, fieldOrderBy), pe);

                    var fieldXExpression = Expression.Property(pe, groupBy);
                    var lambda = Expression.Lambda<Func<T, object>>(
                        fieldXExpression,
                        pe);
                    if (ascending)
                    {
                        var data = await _dbContext.Set<T>().Where(predicate).OrderBy(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<T>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                    else
                    {
                        var data = await _dbContext.Set<T>().Where(predicate).OrderByDescending(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<T>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                }
                else
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, string>>(Expression.Property(pe, fieldOrderBy), pe);
                    var fieldXExpression = Expression.Property(pe, groupBy);
                    var lambda = Expression.Lambda<Func<T, object>>(
                       fieldXExpression,
                        pe);
                    if (ascending)
                    {
                        var data = await _dbContext.Set<T>().Where(predicate).OrderBy(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<T>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                    else
                    {
                        var data = await _dbContext.Set<T>().Where(predicate).OrderByDescending(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<T>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<T, bool>> predicate, string groupBy, string fieldOrderBy, bool ascending, int take, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get number of entity satisfy condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<int> GetCount(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dbContext.Set<T>().Where(predicate).CountAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetCount(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return -1;
            }
        }

        /// <summary>
        /// Get list of entity satisfy condition
        /// </summary>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAll(Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAll(Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get an entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<T> GetById(object id, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetById(object id, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get one entity satisfy condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<T> GetOne(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetOne(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get one entity satisfy condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dbContext.Set<T>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetOne(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get list of entity by store procedure
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Get(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)
        {
            try
            {
                if (parameters != null)
                {
                    var query = string.Concat("Exec ", storedProcedureName, " ");

                    foreach (var item in parameters)
                    {
                        var itemObject = (SqlParameter)item;
                        query += string.Concat(itemObject.ParameterName, ",");
                    }
                    query = parameters.Length > 0 ? query.Substring(0, query.Length - 1) : storedProcedureName;

                    return await _dbContext.Set<T>().FromSql(query, parameters).ToListAsync();
                }
                else
                {
                    return await _dbContext.Set<T>().FromSql(storedProcedureName).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "Get(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get one entity by store procedure
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<T> GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)
        {
            try
            {
                if (parameters != null)
                {
                    var query = string.Concat("Exec ", storedProcedureName, " ");

                    foreach (var item in parameters)
                    {
                        var itemObject = (SqlParameter)item;
                        query += string.Concat(itemObject.ParameterName, ",");
                    }
                    query = parameters.Length > 0 ? query.Substring(0, query.Length - 1) : storedProcedureName;

                    return await _dbContext.Set<T>().FromSql(query, parameters).FirstOrDefaultAsync();
                }
                else
                {
                    return await _dbContext.Set<T>().FromSql(storedProcedureName).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get output of store procedure
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<SqlParameter>> GetOutPut(string storedProcedureName, SqlParameter[] parameters, Ref<CheckError> checkError = null)
        {
            try
            {
                if (parameters != null)
                {
                    var query = string.Concat("", storedProcedureName, " ");

                    var listParameterOutPut = new List<SqlParameter>();

                    foreach (var item in parameters)
                    {
                        var itemObject = (SqlParameter)item;

                        if (itemObject.Direction == ParameterDirection.Output)
                        {
                            listParameterOutPut.Add(itemObject);
                            query += string.Concat(itemObject.ParameterName, " OUT,");
                        }
                        else
                        {
                            query += string.Concat(itemObject.ParameterName, ",");
                        }
                    }
                    query = parameters.Length > 0 ? query.Substring(0, query.Length - 1) : storedProcedureName;

                    await _dbContext.Database.ExecuteSqlCommandAsync(query, parameters);

                    return listParameterOutPut;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetOutPut(string storedProcedureName, SqlParameter[] parameters, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        public async Task<bool> Delete(T entity, Ref<CheckError> checkError = null)
        {
            try
            {
                _dbContext.Set<T>().Remove(entity);
                return await _dbContext.SaveChangesAsync() > 0;
            }




































































            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }

        /// <summary>
        /// Delete model in DataBase
        /// </summary>
        /// <param name="id">Key in entity</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>Boolean</returns>
        public async Task<bool> Delete(object id, Ref<CheckError> checkError = null)
        {
            try
            {
                T entity = await GetById(id);
                if (entity != null)
                {
                    _dbContext.Set<T>().Remove(entity);
                    return await _dbContext.SaveChangesAsync() > 0;
                }
                return true;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }

        /// <summary>
        /// Delete multiple records
        /// </summary>
        /// <param name="list">list entity</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>Boolean</returns>
        public async Task<bool> DeleteAll(IList<T> list, Ref<CheckError> checkError = null)
        {
            try
            {
                _dbContext.Set<T>().RemoveRange(list);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }

        //public async Task<PaginatedList<T>> GetWithPaging(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        //  string includeProperties = "", int page = 1, int pageSize = 2)
        //{
        //    var query = await GetWith(filter: filter, includeProperties: includeProperties);
        //    if (orderBy != null)
        //    {
        //        query = orderBy(query);
        //    }

        //    return PaginatedList<T>.Create(query.AsNoTracking(), page, pageSize);
        //}

        //public async virtual Task<IQueryable<T>> GetWith(Expression<Func<T, bool>> filter = null,
        //   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        //   string includeProperties = "")
        //{
        //    IQueryable<T> query = _dbSet;
        //    if (filter != null)
        //    {
        //        query =  query.Where(filter);
        //    }

        //    foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty);
        //    }

        //    return orderBy != null ? orderBy(query) : query;
        //}
        #endregion
    }
}
