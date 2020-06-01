# **Pandora Mobile Database**
## Getting Started

### Introduction

The class AppDatabase.cs is the main script component, add this script to the component it will use as the database in the main scenes.
This component will be singleton and will last for the duration of the game.

### Overview

* Execute Custom sentence to the database
* Config class table 
* Create table from entity
* Insert row from entity
* Update row from entity
* Check row from entity
* Check custom from  entity table
 
#### *Execute Custom sentence to the database*
```
string creationTable = @"CREATE TABLE IF NOT EXISTS 'TestTable' ( 'id' INTEGER PRIMARY KEY,  
                                    'name' TEXT NOT NULL,
                                    'version' INTEGER NOT NULL
									 );";
bool result = AppDatabase.Instance.ExecuteCreationTable(creationTable);
```

#### *Config class table*

```
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
```

#### *Insert row from entity*

```
AppInfo inf = new AppInfo
{
    Name = "manolio",
    NumberFloat = 15f,
    NumberInt = 30,
    Version = "0.0.1"
};

 bool insertEntity  = AppDatabase.Instance.EntityInsert<AppInfo>(inf);
```

#### *Update row from entity*

```
AppInfo inf = new AppInfo
{
    Id = 1,
    Name = "manolio",
    NumberFloat = 15f,
    NumberInt = 30,
    Version = "0.0.1"
};
        
 bool updateResult =  AppDatabase.Instance.EntityUpdate<AppInfo>(inf);
```

#### *Check row from entity*

```
Dictionary<string, object> applinfo = AppDatabase.Instance.EntityGet<AppInfo>(1, new AppInfo());
```

#### *Check custom from  entity table*

```
Condition cond = new Condition();
cond.AddWhereWithParam("NumberFloat", 15);
List<IEntity> listadoCond = AppDatabase.Instance.GetWithCondiction<IEntity>(new AppInfo(), cond);
```
