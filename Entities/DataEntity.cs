namespace Entities;

public class DataEntity<T>
    {
        private readonly string key;
        private readonly T value;
        private readonly DateTime expirationDateTime;

        public DataEntity(string key, T value, int expirationInterval)
        {
            this.key = key;
            this.value = value;
            DateTime currentDate = DateTime.Now;
            expirationDateTime = currentDate.AddHours(expirationInterval);
        }

        public string GetKey()
        {
            return key;
        }

        public T GetValue()
        {
            return value;
        }

        public DateTime GetExpirationDateTime()
        {
            return expirationDateTime;
        }

        public bool IsExpired()
        {
            return DateTime.Now > expirationDateTime;
        }
    }