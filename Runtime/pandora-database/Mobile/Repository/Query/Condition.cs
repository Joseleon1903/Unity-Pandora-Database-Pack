using System.Collections.Generic;
using Unity.Pandora.Database.Mobile.Repository.Enum;

namespace Unity.Pandora.Database.Mobile.Repository.Query
{
    public class Condition
    { 

        public Dictionary<ConditionKey, string> QueryCondiction { get; }

        public Condition()
        {
            QueryCondiction = new Dictionary<ConditionKey, string>();
        }

        public void AddOrderBy(string columName, OrderType orderType)
        {

            string content = " ORDER BY " + columName;
            if (orderType.Equals(OrderType.ASC))
            {
                content += " ASC ";
            }
            if (orderType.Equals(OrderType.DESC))
            {
                content += " DESC ";
            }
            QueryCondiction.Add(ConditionKey.OREDER_BY, content);
        }

        public void AddLimit(int rowLimit)
        {
            string content = " LIMIT " + rowLimit + " ";
            QueryCondiction.Add(ConditionKey.LIMIT, content);
        }


        public void AddWhereWithParam(string columnName, object columnValue)
        {
            string content = " Where ";
            if (typeof(string).IsInstanceOfType(columnValue))
            {
                content += columnName + " ='" + columnValue + "' ";
            }
            else {
                content += columnName + " =" + columnValue +" ";
            }
            QueryCondiction.Add(ConditionKey.WHERE, content);
        }
    }
}