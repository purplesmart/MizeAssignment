using Interfaces;

namespace Storages;
    public abstract class StorageInChainBase<T> : IStorage<T> where T : ICloneable
    {
        private int expirationInterval;
        private int depth;
        private IStorage<T> nextStorage;
        protected bool isReadOnly = false;

        public int ExpirationInterval => expirationInterval;
        public int Depth => depth;

        public bool IsReadOnly() => isReadOnly;

        public void Propagate(string key, T value)
        {
            if(!IsReadOnly()){
                AddValue(key,value);
            }
            if(GetNextStorage() != null)
            {
                GetNextStorage().Propagate(key,value);
            }
        }

        public StorageInChainBase(int depth, int expirationInterval, bool viaClone)
        {
            this.depth = depth;
            this.expirationInterval = expirationInterval;
            if(!viaClone)
            {
            IStorage<T> nextStorage = this;

            for (int i = 0; i < depth - 1; i++)
            {
                IStorage<T> storageWrapper = new StorageWrapper<T>((StorageInChainBase<T>)nextStorage.Clone());
                storageWrapper = storageWrapper.GetStorage();
                nextStorage.SetNextStorage(storageWrapper);
                nextStorage = storageWrapper;
            }

            ((StorageInChainBase<T>)nextStorage).isReadOnly = true;
            }
        }

        public IStorage<T> GetNextStorage() => nextStorage;

        public void SetNextStorage(IStorage<T> nextStorage) => this.nextStorage = nextStorage;

        public void AddValue(string key, T value, int depth)
        {
            IStorage<T> currentStorage = this;
            int counter = 1;

            while (depth != counter)
            {
                if (currentStorage.GetNextStorage() == null)
                {
                    throw new ArgumentOutOfRangeException("Depth too deep for the chain");
                }
                currentStorage = currentStorage.GetNextStorage();
                counter++;
            }

            if (!isReadOnly)
            {
                currentStorage.AddValue(key, value);
            }
        }

        public abstract T GetValue(string key);

        public abstract void AddValue(string key, T value);

        public abstract void Init();

        public abstract bool ValueExists(string key);

        public abstract IStorage<T> Clone();

        public abstract IStorage<T> GetStorage();

    }
