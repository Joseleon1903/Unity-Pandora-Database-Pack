using System.Collections.Generic;
using Unity.Pandora.Database.Mobile.Repository.Helper;
using Unity.Pandora.Database.Mobile.Repository.Query;
using UnityEngine;

namespace Unity.Pandora.Database.Mobile.Repository.Config
{
    public class EntityRepository<TEntity> where TEntity : IEntity
    {
        protected IEntity entity;

        private ApplicationPlatformContext<TEntity> contextEntity;
        public EntityRepository(ApplicationPlatformContext<TEntity> contextEntity, IEntity entity, bool isCreated)
        {
            this.contextEntity = contextEntity;
            this.entity = entity;
            if (isCreated)
            {
                CreateTable();
            }
        }

        private void CreateTable()
        {
            string creationQuery = SqliteHelper.CreatedTableFromEntity(entity);
            Debug.Log("creationQuery");
            contextEntity.CreationTable(creationQuery);
        }

        public void Add(IEntity entity)
        {
            string insertQuery;

            int id = int.Parse(entity.GetDictionary()["Id"].ToString().Trim());
            if (id > 0)
            {
                insertQuery = SqliteHelper.InsertEntityById(entity);
            }
            else
            {
                insertQuery = SqliteHelper.InsertEntity(entity);
            }
            LoggerHelper.LogConsole(insertQuery);
            contextEntity.ExecuteInsert(insertQuery);
        }

        public Dictionary<string, object> FindOne(int id)
        {
            string select = SqliteHelper.SelectEntity(entity.GetTableName(), entity.GetDictionary(), id);
            LoggerHelper.LogConsole(select);
            return contextEntity.ExecuteSelectEntity(select, entity.GetDictionary()); ;
        }

        public void Update(IEntity entity)
        {
            string update = SqliteHelper.UpdateEntity(entity);
            LoggerHelper.LogConsole(update);
            contextEntity.ExecuteInsert(update);
        }

        public List<string> FindAll(IEntity entity)
        {
            string select = SqliteHelper.SelectAllEntity(entity.GetTableName(), entity.GetDictionary());
            LoggerHelper.LogConsole(select);
            List<string> list = contextEntity.ExecuteSelectAllEntity(select);
            return list;
        }

        public List<string> FindByCondiction(IEntity entity, Condition condition)
        {
            string select = SqliteHelper.SelectWithCondiction(entity, condition.QueryCondiction);
            LoggerHelper.LogConsole(select);
            List<string> list = contextEntity.ExecuteSelectAllEntity(select);
            return list;
        }

        public void Delete(int id)
        {
            string delete = SqliteHelper.DeleteEntity(id, entity);
            LoggerHelper.LogConsole(delete);
            contextEntity.ExecuteInsert(delete);
        }

        internal Dictionary<string, object> FindOneByCondiction(IEntity entity, Condition condition)
        {
            string select = SqliteHelper.SelectWithCondiction(entity, condition.QueryCondiction);
            LoggerHelper.LogConsole(select);
            Dictionary<string, object> result = contextEntity.ExecuteSelectEntity(select, entity.GetDictionary());
            return result;
        }
    }
}