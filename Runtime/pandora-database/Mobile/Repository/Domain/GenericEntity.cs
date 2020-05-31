using System.Collections.Generic;
using Unity.Pandora.Database.Mobile.Repository.Config;

namespace Unity.Pandora.Database.Mobile.Repository.Domain
{
    public class GenericEntity : IEntity
    {
        public string[] GetColumns()
        {
            return new string[] { };
        }

        public Dictionary<string, object> GetDictionary()
        {
            throw new System.NotImplementedException();
        }

        public IEntity GetInstance()
        {
            return new GenericEntity();
        }

        public string GetTableName()
        {
            return "";
        }

        public void SetDictionary(Dictionary<string, object> objectData)
        {
            throw new System.NotImplementedException();
        }

    }
}