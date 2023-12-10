using Interfaces;

namespace Storages;
    public class StorageWrapper<T> : IStorage<T>
    {
        private IStorage<T> storage;

        public StorageWrapper(IStorage<T> storage)
        {
            this.storage = storage;
        }

        public IStorage<T> GetStorage()
        {
            return storage;
        }

        public T GetValue(string key)
        {
            return storage.GetValue(key);
        }

        public IStorage<T> GetNextStorage()
        {
            return storage.GetNextStorage();
        }

        public void SetNextStorage(IStorage<T> nextStorage)
        {
            storage.SetNextStorage(nextStorage);
        }

        public void AddValue(string key, T value)
        {
            try
            {
                if (!IsReadOnly())
                {
                    storage.AddValue(key, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddValue(string key, T value, int depth)
        {
            try
            {
                storage.AddValue(key, value, depth);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool IsReadOnly()
        {
            return storage.IsReadOnly();
        }

        public void Init()
        {
            storage.Init();
        }

        public void Propagate(string key, T value)
        {
            try
            {
                storage.Propagate(key, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool ValueExists(string key)
        {
            try
            {
                return storage.ValueExists(key);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IStorage<T> Clone()
        {
            return storage.Clone();
        }
    }

