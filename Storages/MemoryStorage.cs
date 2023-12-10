using Entities;
using Interfaces;

namespace Storages;
public class MemoryStorage : StorageInChainBase<string>
    {
        private Dictionary<string, DataEntity<string>> dataSource;

        public MemoryStorage(int depth, int expirationInterval, bool viaClone = false) : base(depth, expirationInterval, viaClone)
        {
            Init();
        }

        public override void Init()
        {
            dataSource = new Dictionary<string, DataEntity<string>>();
        }

        public override bool ValueExists(string key)
        {
            return dataSource.ContainsKey(key);
        }

        public override string GetValue(string key)
        {
            DataEntity<string> content = null;
            do
            {
                if (dataSource.ContainsKey(key))
                {
                    content = dataSource[key];
                }

                if (content != null && !content.IsExpired())
                {
                    string value = content.GetValue();
                    Propagate(key, value);
                    return value;
                }

                if (GetNextStorage() != null)
                {
                    return GetNextStorage().GetValue(key);
                }
            } while (GetNextStorage() != null);

            throw new InvalidOperationException("No result for key: " + key);
        }

        public override void AddValue(string key, string value)
        {
            dataSource.Remove(key);
            dataSource[key] = new DataEntity<string>(key, value, ExpirationInterval);
        }

        public override IStorage<string> Clone()
        {
            return new MemoryStorage(Depth,ExpirationInterval, true);
        }

        public override IStorage<string> GetStorage()
        {
            return this;
        }
    }
