using Entities;
using Interfaces;
using Newtonsoft.Json;

namespace Storages;
    public class FileSystemStorage : StorageInChainBase<JsonDumb>
    {
        private string folderPath;
        private Guid uniqueID;
        private string filePathMize;

        private Dictionary<string, DataEntity<string>> dataSource;

        private readonly JsonSerializerSettings jsonSettings;

        public FileSystemStorage(int depth, int expirationInterval, bool viaClone = false) : base(depth, expirationInterval,viaClone)
        {
            Init();
            jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public override void Init()
        {
            dataSource = new Dictionary<string, DataEntity<string>>();
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            uniqueID = Guid.NewGuid();
            filePathMize = "MizeFiles";
        }

        public override bool ValueExists(string key)
        {
            return dataSource.ContainsKey(key);
        }

        public override JsonDumb GetValue(string key)
        {
            DataEntity<string> dataEntity = null;
            do
            {
                if (dataSource.ContainsKey(key))
                {
                    dataEntity = dataSource[key];
                }

                if (dataEntity != null && !dataEntity.IsExpired())
                {
                    JsonDumb jsonDumb = LoadJSONFile(dataEntity.GetValue());
                    Propagate(key, jsonDumb);
                    return jsonDumb;
                }

                if (GetNextStorage() != null)
                {
                    return GetNextStorage().GetValue(key);
                }
            } while (GetNextStorage() != null);

            throw new InvalidOperationException("No result for key: " + key);
        }

        private JsonDumb LoadJSONFile(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonDumb jsonDumb = (JsonDumb)serializer.Deserialize(file, typeof(JsonDumb));
                return jsonDumb;
            }
        }

        public override void AddValue(string key, JsonDumb value)
        {
            string filePath;
            if (dataSource.ContainsKey(key))
            {
                DataEntity<string> dataEntity = dataSource[key];
                filePath = dataEntity.GetValue();
                dataSource.Remove(key);
            }
            else
            {
                filePath = SaveFileReturnFullPath(key, value);
            }

            dataSource[key] = new DataEntity<string>(key, filePath, ExpirationInterval);
        }

        private string SaveFileReturnFullPath(string key, JsonDumb jsonDumb)
        {
            string path = Path.Combine(folderPath, filePathMize, uniqueID.ToString() + key + ".json");
            string directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);

            string json = JsonConvert.SerializeObject(jsonDumb, jsonSettings);
            File.WriteAllText(path, json);

            return path;
        }

        public override IStorage<JsonDumb> Clone()
        {
            return new FileSystemStorage(Depth,ExpirationInterval,true);
        }

        public override IStorage<JsonDumb> GetStorage()
        {
            return this;
        }
    }

