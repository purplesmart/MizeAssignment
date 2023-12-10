namespace Interfaces;
public interface IStorage<T>
    {
        T GetValue(string key);
        IStorage<T> GetNextStorage();
        void SetNextStorage(IStorage<T> nextStorage);
        void AddValue(string key, T value);
        void AddValue(string key, T value, int depth);
        bool IsReadOnly();
        void Init();
        void Propagate(string key, T value);
        bool ValueExists(string key);
        IStorage<T> GetStorage();
        IStorage<T> Clone();
}