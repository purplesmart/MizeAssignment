using Interfaces;
using Storages;
using Xunit;

namespace MizeTests;

public class WebServiceStorageTest
{
        private readonly int depthCount = 5;
        private readonly int expirationInterval = 3;

        [Fact]
        public void StorageChainLength()
        {
            int count = 0;
            IStorage<string> memoryStorage = new MemoryStorage(depthCount, expirationInterval);
            while (memoryStorage != null)
            {
                memoryStorage = memoryStorage.GetNextStorage();
                count++;
            }
            Assert.Equal(depthCount, count);
        }

        [Fact]
        public void StorageInChainNotReadOnly()
        {
            IStorage<string> memoryStorage = new MemoryStorage(depthCount, expirationInterval);
            while (memoryStorage.GetNextStorage() != null)
            {
                Assert.False(memoryStorage.IsReadOnly());
                memoryStorage = memoryStorage.GetNextStorage();
            }
            Assert.True(memoryStorage.IsReadOnly());
        }

        [Fact]
        public void GetNotValidValue()
        {
            string notValidKey = "NONVALIDKEY";
            Assert.Throws<InvalidOperationException>(() =>
            {
                IStorage<string> memoryStorage = new MemoryStorage(depthCount, expirationInterval);
                memoryStorage.GetValue(notValidKey);
            });
        }

        [Fact]
        public void GetValueNoDepth()
        {
            string validKey = "VALIDKEY";
            string validValue = "VALIDVALUE";
            IStorage<string> memoryStorage = new MemoryStorage(depthCount, expirationInterval);
            memoryStorage.AddValue(validKey, validValue);
            string value = memoryStorage.GetValue(validKey);
            Assert.Equal(validValue, value);
        }

        [Fact]
        public void GetValueWithDepth()
        {
            string validKey = "VALIDKEY";
            string validValue = "VALIDVALUE";
            int validDepth = 5;
            IStorage<string> memoryStorage = new MemoryStorage(depthCount, expirationInterval);
            memoryStorage.AddValue(validKey, validValue, validDepth);
            string value = memoryStorage.GetValue(validKey);
            Assert.Equal(validValue, value);
        }

        [Fact]
        public void GetValueWithIllegalDepth()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                string validKey = "VALIDKEY";
                string validValue = "VALIDVALUE";
                int invalidDepth = 6;
                IStorage<string> memoryStorage = new MemoryStorage(depthCount, expirationInterval);
                memoryStorage.AddValue(validKey, validValue, invalidDepth);
            });
        }

        [Fact]
        public void CheckPropagate()
        {
            string validKey = "VALIDKEY";
            string validValue = "VALIDVALUE";
            int validDepth = 3;
            IStorage<string> memoryStorage = new MemoryStorage(depthCount, expirationInterval);
            memoryStorage.AddValue(validKey, validValue, validDepth);
            memoryStorage.GetValue(validKey);
            IStorage<string> currentStorage = memoryStorage;
            for (int i = 1; i < depthCount; i++)
            {
                if (i < validDepth)
                {
                    Assert.False(currentStorage.ValueExists(validKey));
                }
                else
                {
                    Assert.True(currentStorage.ValueExists(validKey));
                }
                currentStorage = currentStorage.GetNextStorage();
            }
        }
}
