using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using SMEasy.Common;
using SMEasy.Data;
 
using Xen.Common.Services.Logging;
using Xen.Entity;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.SqlClient;
using Xen.Resources;
using SMEasy.Resource;

namespace SMEasy.WebApi.Controllers
{

    public class BaseController<TEntity> : ApiController  where TEntity : BaseEFEntity
    {
        internal readonly SMEasyEntities DbContext = new SMEasyEntities();

        #region HTTP GET
        public virtual List<TEntity> Get()
        {
            return GetList();
        }

        public virtual TEntity Get(long id)
        {
            try
            {
                CrudResult<TEntity> result = this.GetEntityById(id);
                return this.PrepareResultForGetOperation(result);
            }
            catch (HttpResponseException httpException)
            {
                throw httpException.GetBaseException();
            }
        }

        #endregion HTTP GET


        #region HTTP PUT
        [HttpPut]
        public virtual HttpResponseMessage Put(long id, TEntity entity)
        {
            return Save(entity);
        }
        #endregion HTTP PUT

        #region HTTP POST
        [HttpPost]
        public virtual HttpResponseMessage Post(TEntity entity)
        {
            return Save(entity);
        }

        #endregion HTTP POST

        #region HTTP DELETE
        public virtual HttpResponseMessage Delete(long id)
        {
            try
            {
                var entity = Get(id);
                CrudResult<TEntity> result = DeleteEntity(entity);
                HttpResponseMessage response = this.PrepareResultForSaveOperation(result);
                return response;
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, exception.Message);
            }
        }

        #endregion HTTP DELETE

        private List<TEntity> GetList()
        {
            try
            {
                return PrepareResultForGetListOperation(this.Get(orderBy: i => i.OrderByDescending(s => s.Id)));
            }
            catch (HttpResponseException httpException)
            {
                throw httpException.GetBaseException();
            }
           
        }

       private CrudResult<List<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "", bool isSearch = false, bool getAllRecords = false)
        {
            CrudResult<List<TEntity>> result = new CrudResult<List<TEntity>>();
            try
            {
                DbSet<TEntity> dbSet = this.DbContext.Set<TEntity>();
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                    query = query.Where(filter);

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                //need to get all records for menu and page related stuff                
                if (getAllRecords)
                    result.Result = (orderBy != null) ? orderBy(query).ToList() : query.OrderByDescending(d => d.Id).ToList();
                else
                {
                    //if the isSearch== true, then its search query, so system will take top 100 records
                    result.Result = (isSearch) ? (orderBy != null) ? orderBy(query).Take(100).ToList() : query.OrderByDescending(d => d.Id).Take(100).ToList() :
                        (orderBy != null) ? orderBy(query).Take(25).ToList() : query.OrderByDescending(d => d.Id).Take(25).ToList();
                }

                result.CrudStatus = CrudStatusType.DataSelectedSuccessfully;
            }
            catch (Exception)
            {
                //PopulateCrudResultFromException(result, exception);
            }
            return result;
        }

       private CrudResult<TEntity> GetEntityById(long id, string includeProperties = "")
       {
           CrudResult<TEntity> result = new CrudResult<TEntity>();
           try
           {
               DbSet<TEntity> dbSet = this.DbContext.Set<TEntity>();
               IQueryable<TEntity> query = dbSet;

               foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
               {
                   query = query.Include(includeProperty);
               }
               //result.Result = this.dbContext.Set<TEntity>().Find(id);
               result.Result = query.Where(i => i.Id == id).FirstOrDefault();
               result.CrudStatus = CrudStatusType.DataSelectedSuccessfully;
           }
           catch (Exception exception)
           {
               PopulateCrudResultFromException(result, exception);
           }
           return result;
       }

        internal List<TEntity> PrepareResultForGetListOperation(CrudResult<List<TEntity>> result)
        {
            switch (result.CrudStatus)
            {
                case CrudStatusType.DataSelectedSuccessfully:
                    return result.Result;
                case CrudStatusType.ExceptionExists:
                    throw new HttpResponseException(new
               HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent(result.AdditionalInfo)
                    });
            }
            return new List<TEntity>();
        }

        internal TEntity PrepareResultForGetOperation(CrudResult<TEntity> result)
        {
            return result.Result;
        }

        private CrudResult<TEntity> DeleteEntity(TEntity entity)
        {
            return this.SaveChanges(entity, EntityState.Deleted);
        }

        private HttpResponseMessage Save(TEntity entity)
        {
            try
            {
                CrudResult<TEntity> result = default(CrudResult<TEntity>);
                if (TryPreparingForAdd(entity))
                {
                    EntityState entityState = (entity.Id == 0 ? EntityState.Added : EntityState.Modified);
                    result = SaveChanges(entity, entityState);
                    HttpResponseMessage response = this.PrepareResultForSaveOperation(result);
                    return response;
                }
                return Request.CreateResponse<BaseEFEntity>(HttpStatusCode.BadRequest, entity);
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, exception.Message);
            }
        }

        private CrudResult<TEntity> SaveChanges(TEntity entity, EntityState entityState)
        {
            CrudResult<TEntity> result = new CrudResult<TEntity>();
            try
            {
                switch (entityState)
                {
                    case EntityState.Added:
                        this.DbContext.Set<TEntity>().Add(entity);
                        break;
                    case EntityState.Deleted:
                        if (this.DbContext.Entry(entity).State == EntityState.Detached)
                            this.DbContext.Set<TEntity>().Attach(entity);
                        this.DbContext.Set<TEntity>().Remove(entity);

                        break;
                    case EntityState.Modified:
                        this.DbContext.Set<TEntity>().Attach(entity);
                        this.DbContext.Entry(entity).State = EntityState.Modified;
                        break;
                    case EntityState.Unchanged:
                        break;
                }
                result.RowsAffected = this.DbContext.SaveChanges();
                PopulateCrudResultForSave(result, entity, entityState);
            }
            catch (Exception exception)
            {
                PopulateCrudResultFromException(result, exception);
            }
            return result;
        }

        internal bool TryPreparingForAdd(TEntity entity)
        {

            bool isModelValid = entity.IsValid;

            if (!isModelValid) return isModelValid;
             
            if (entity.IsNewRecord)
            {
                entity.CreationTs = DateTime.Now;
                entity.CreationUserId = "CurrentUser";
                entity.StatusType = StatusType.Enabled;
            }
            else
            {
                entity.LastChangeTs = DateTime.Now;
                entity.LastChangeUserId = "CurrentUser";
            }
            return true;
        }

        private CrudResult<TEntity> PopulateCrudResultForSave(CrudResult<TEntity> result, TEntity entity, EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Added:
                    if (result.RowsAffected > 0)
                    {
                        result.CrudStatus = CrudStatusType.DataAddedSuccessfully;
                    }
                    else
                    {
                        result.CrudStatus = CrudStatusType.DataNotAdded;
                    }
                    break;
                case EntityState.Deleted:
                    if (result.RowsAffected > 0)
                    {
                        result.CrudStatus = CrudStatusType.DataDeletedSuccessfully;
                    }
                    else
                    {
                        result.CrudStatus = CrudStatusType.DataNotDeleted;
                    }
                    break;
                case EntityState.Modified:
                    if (result.RowsAffected > 0)
                    {
                        result.CrudStatus = CrudStatusType.DataUpdatedSuccessfully;
                    }
                    else
                    {
                        result.CrudStatus = CrudStatusType.DataNotUpdated;
                    }
                    break;
                case EntityState.Unchanged:
                    result.CrudStatus = CrudStatusType.Unknown;
                    break;
            }
            result.Result = entity;
            return result;
        }
        
        private CrudResult PopulateCrudResultFromException(CrudResult result, Exception exception)
        {
            //XenLoggerService.LogException(exception);
            PopulateExceptionInfo(result, exception);
            return result;
        }

        internal HttpResponseMessage PrepareResultForSaveOperation(CrudResult<TEntity> result)
        {
            if (result == null) return new HttpResponseMessage(HttpStatusCode.BadRequest);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unused);
            switch (result.CrudStatus)
            {
                case CrudStatusType.DataAddedSuccessfully:
                    response = Request.CreateResponse(HttpStatusCode.Created, result.Result);
                    response.Headers.Location = new Uri(Url.Link("ApiById", new { id = result.Result.Id }));
                    break;
                case CrudStatusType.DataUpdatedSuccessfully:
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    break;
                case CrudStatusType.DataDeletedSuccessfully:
                    response = Request.CreateResponse(HttpStatusCode.OK, result.Result);
                    break;
                case CrudStatusType.DataNotDeleted:
                    response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    break;
                case CrudStatusType.ExceptionExists:
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(result.AdditionalInfo) };
                    break;
                case CrudStatusType.DataNotAdded:
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    break;
            }

            return response;
        }

        private void PopulateExceptionInfo(CrudResult result, Exception exception)
        {
            if (exception == null) return;

            Exception rootExcpetion = exception.GetBaseException();

            if (rootExcpetion == null) return;

            result.AdditionalInfo += string.Format("\nAdditional Information : {0}", rootExcpetion.Message);

            #region Check for Sql Exception
            SqlException sqlException = rootExcpetion as System.Data.SqlClient.SqlException;
            string exceptionMessage = rootExcpetion.Message;
            if (sqlException != null)
            {
                string sqlErrorMessage = SqlErrorMessages.ResourceManager.GetString(sqlException.Number.ToString(), SqlErrorMessages.Culture);

                if (sqlException.Number == 2601) //cannot insert duplicate record
                {
                    var startPos = exceptionMessage.IndexOf(@"with unique index '");
                    var endPos = exceptionMessage.IndexOf(@"'.", startPos);
                    startPos += "with unique index '".Length;
                    var indexName = exceptionMessage.Substring(startPos, (endPos - startPos));
                    var qualifiedIndexName = IndexColumnName.ResourceManager.GetString(indexName, SqlErrorMessages.Culture);
                    sqlErrorMessage = string.Format(sqlErrorMessage, (qualifiedIndexName == null ? indexName : qualifiedIndexName));
                }
                else if (sqlException.Number == 547) //reference key error
                {
                    var startPos = exceptionMessage.IndexOf(@", table ");
                    var endPos = exceptionMessage.IndexOf(", column '");
                    startPos += ", table ".Length;
                    var referenceTableName = exceptionMessage.Substring(startPos, (endPos - startPos));
                    sqlErrorMessage = string.Format(sqlErrorMessage, referenceTableName);
                }

                result.AdditionalInfo = (string.IsNullOrEmpty(sqlErrorMessage) ? result.AdditionalInfo : sqlErrorMessage);
            }
            #endregion Check for Sql Exception

            result.CrudStatus = CrudStatusType.ExceptionExists;
            result.ExceptionInfo = exception.StackTrace;
        }
    }
}