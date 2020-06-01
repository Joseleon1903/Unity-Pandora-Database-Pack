using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using Unity.Pandora.Database.Mobile.Repository.Config;
using Unity.Pandora.Database.Mobile.Repository.Domain;
using Unity.Pandora.Database.Mobile.Repository.Enum;
using Unity.Pandora.Database.Mobile.Repository.ExceptionTypes;
using Unity.Pandora.Database.Mobile.Repository.Helper;
using Unity.Pandora.Database.Mobile.Repository.Query;
using UnityEngine;

/// <summary>
/// 
///  AppDatabase component for sqlite database implementation
/// 
/// </summary>
namespace Unity.Pandora.Database.Mobile.Repository {

    public class AppDatabase : MonoBehaviour
    {
        public static AppDatabase Instance { get; private set; }

        private ApplicationPlatformContext<GenericEntity> Context = new ApplicationPlatformContext<GenericEntity>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
                Context.Initialized();
            }
            DontDestroyOnLoad(this);
        }

        // serialized file config
        public LoggerType loggerType;

        public string DbName = "BaseDataBase.s3db";


        //Methods

        public bool ExecuteCreationTable(string statement)
        {
            bool success = false;
            try
            {
                Context.CreationTable(statement);
                success= true;
            }
            catch (SqliteException ex) {
                SqLiteExceptionHandler<SqliteException> error = new SqLiteExceptionHandler<SqliteException>(ex);
                LoggerHelper.LogConsole("Database Error : ");
                LoggerHelper.LogConsole("Description" + error.GetDescription());
                LoggerHelper.LogConsole("TrackTrace" + error.GetTrackTrace());
            }
            return success;
        }

        public bool ExecuteSentence(string insert)
        {
            bool success = false;
            try
            {
                Context.ExecuteInsert(insert);
                success = true;
            }
            catch (SqliteException ex)
            {
                SqLiteExceptionHandler<SqliteException> error = new SqLiteExceptionHandler<SqliteException>(ex);
                LoggerHelper.LogConsole("Database Error : ");
                LoggerHelper.LogConsole("Description" + error.GetDescription());
                LoggerHelper.LogConsole("TrackTrace" + error.GetTrackTrace());
            }
            return success;
        }

        [Obsolete("ExecuteSelect is deprecated and unsafe, please contruct entity and use method ExecuteSelectEntity.")]
        public string ExecuteSelect(string select, string[] columns)
        {
            string result = Context.ExecuteSelect(select, columns);
            return result;
        }

        public bool EntityInsert<TEntity>(IEntity entity) where TEntity : IEntity
        {
            bool success = false;
            try
            {
                ApplicationPlatformContext<TEntity> ContextEntity = new ApplicationPlatformContext<TEntity>();
                ContextEntity.Initialized();
                EntityRepository<TEntity> repository = new EntityRepository<TEntity>(ContextEntity, entity, true);
                repository.Add(entity);
                success = true;
            }
            catch (SqliteException ex)
            {
                SqLiteExceptionHandler<SqliteException> error = new SqLiteExceptionHandler<SqliteException>(ex);
                LoggerHelper.LogConsole("Database Error : ");
                LoggerHelper.LogConsole("Description" + error.GetDescription());
                LoggerHelper.LogConsole("TrackTrace" + error.GetTrackTrace());
            }
            return success;
        }

        public bool EntityUpdate<TEntity>(IEntity entity) where TEntity : IEntity
        {
            bool success = false;
            try
            {
                ApplicationPlatformContext<TEntity> ContextEntity = new ApplicationPlatformContext<TEntity>();
                ContextEntity.Initialized();
                EntityRepository<TEntity> repository = new EntityRepository<TEntity>(ContextEntity, entity, false);
                repository.Update(entity);
                success = true;
            }
            catch (SqliteException ex)
            {
                SqLiteExceptionHandler<SqliteException> error = new SqLiteExceptionHandler<SqliteException>(ex);
                LoggerHelper.LogConsole("Database Error : ");
                LoggerHelper.LogConsole("Description" + error.GetDescription());
                LoggerHelper.LogConsole("TrackTrace" + error.GetTrackTrace());
            }
            return success;
        }

        public List<TEntity> EntityGetAll<TEntity>(TEntity entity) where TEntity : IEntity
        {
            ApplicationPlatformContext<TEntity> ContextEntity = new ApplicationPlatformContext<TEntity>();
            ContextEntity.Initialized();
            EntityRepository<TEntity> repository = new EntityRepository<TEntity>(ContextEntity, entity, false);
            List<string> entityJsonList = repository.FindAll(entity);

            List<TEntity> entities = new List<TEntity>();

            foreach (string item in entityJsonList) {
                entity = (TEntity)entity.GetInstance();
                entity.SetDictionary(JsonHelper.ParseJSON(item));
                entities.Add(entity);
            }
            return entities;
        }

        public Dictionary<string, object> EntityGet<TEntity>(int id, IEntity entity) where TEntity : IEntity
        {
            ApplicationPlatformContext<TEntity> ContextEntity = new ApplicationPlatformContext<TEntity>();
            ContextEntity.Initialized();
            EntityRepository<TEntity> repository = new EntityRepository<TEntity>(ContextEntity, entity, false);
            return repository.FindOne(id);
        }

        public void EntityRemove<TEntity>(int id, IEntity entity) where TEntity : IEntity
        {
            ApplicationPlatformContext<TEntity> ContextEntity = new ApplicationPlatformContext<TEntity>();
            ContextEntity.Initialized();
            EntityRepository<TEntity> repository = new EntityRepository<TEntity>(ContextEntity, entity, false);
            repository.Delete(id);
        }


        public List<TEntity> GetWithCondiction<TEntity>(TEntity entity, Condition condition) where TEntity : IEntity
        {
            ApplicationPlatformContext<TEntity> ContextEntity = new ApplicationPlatformContext<TEntity>();
            ContextEntity.Initialized();
            EntityRepository<TEntity> repository = new EntityRepository<TEntity>(ContextEntity, entity, false);
            List<string> entityJsonList = repository.FindByCondiction(entity, condition);

            List<TEntity> entities = new List<TEntity>();

            foreach (string item in entityJsonList)
            {
                entity = (TEntity)entity.GetInstance();
                entity.SetDictionary(JsonHelper.ParseJSON(item));
                entities.Add(entity);
            }
            return entities;
        }


        public Dictionary<string, object> GetOneWithCondiction<TEntity>(IEntity entity, Condition condition) where TEntity : IEntity
        {
            //default set 1 result
            condition.AddLimit(1);
            ApplicationPlatformContext<TEntity> ContextEntity = new ApplicationPlatformContext<TEntity>();
            ContextEntity.Initialized();
            EntityRepository<TEntity> repository = new EntityRepository<TEntity>(ContextEntity, entity, false);
            Dictionary<string, object> entityOut = repository.FindOneByCondiction(entity, condition);
            return entityOut;
        }

    }

}