using System;
using System.Collections.Generic;
using Unity.Pandora.Database.Mobile.Repository.Config;
using Unity.Pandora.Database.Mobile.Repository.Helper;

namespace Unity.Pandora.Database.Mobile.Repository.Domain
{

    [System.Serializable]
    public class AppInfo : IEntity
    {
        private static string TableName => "AppInfo";

        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public float NumberFloat { get; set; }

        public int NumberInt { get; set; }

        public AppInfo() { }

        public AppInfo(Dictionary<string, object> objectData)
        {
            SetDictionary(objectData);
        }

        public string GetTableName()
        {
            return TableName;
        }

        public Dictionary<string, object> GetDictionary()
        {
            return new Dictionary<string, object>
        {
            { "Id", Id },
            { "Name", Name },
            { "Version", Version },
            { "NumberFloat", NumberFloat},
            { "NumberInt", NumberInt}
        };
        }

        public void SetDictionary(Dictionary<string, object> objectData)
        {
            Id = int.Parse(objectData["Id"].ToString().Trim());
            Name = (string)objectData["Name"].ToString();
            Version = (string)objectData["Version"].ToString();
            NumberFloat = float.Parse(objectData["NumberFloat"].ToString().Trim());
            NumberInt = int.Parse(objectData["NumberInt"].ToString().Trim());
        }

        public static explicit operator AppInfo(List<AppInfo> v)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return JsonHelper.ParserDictionary(GetDictionary());
        }

        public IEntity NewFromDictionary(Dictionary<string, object> objectData)
        {
            return new AppInfo
            {
                Id = int.Parse(objectData["Id"].ToString().Trim()),
                Name = objectData["Name"].ToString(),
                Version = objectData["Version"].ToString(),
                NumberFloat = float.Parse(objectData["NumberFloat"].ToString().Trim()),
                NumberInt = int.Parse(objectData["NumberInt"].ToString().Trim()),
            };
        }

        public IEntity GetInstance()
        {
            return new AppInfo();
        }
    }
}