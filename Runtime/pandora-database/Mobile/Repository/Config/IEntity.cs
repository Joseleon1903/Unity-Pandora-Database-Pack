using System.Collections.Generic;

namespace Unity.Pandora.Database.Mobile.Repository.Config
{
    public interface IEntity
    {
        /// <summary>
        ///  get the table name
        /// </summary>
        /// <returns>string</returns>
        string GetTableName();

        /// <summary>
        ///  get the entity content in  Dictionary<string, object> values
        /// </summary>
        /// <returns>Dictionary<string, object></returns>
        Dictionary<string, object> GetDictionary();


        /// <summary>
        ///  set the entity content 
        /// </summary>
        /// 
        /// <param> Dictionary<string, object> objectData</param>
        /// <returns>Dictionary<string, object></returns>
        void SetDictionary(Dictionary<string, object> objectData);


        IEntity GetInstance();
    }
}