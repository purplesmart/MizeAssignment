using Entities;
using Interfaces;
using Storages;
using Xunit;

namespace MizeTests;

public class FileSystemStorageTest
{

     private readonly int depthCount = 5;
    private readonly int expirationInterval = 3;

    [Fact]
    public void StorageChainLength()
    {
         int count = 0;
        IStorage<JsonDumb> fileSystemStorage = new FileSystemStorage(depthCount, expirationInterval);
        while (fileSystemStorage != null){
            fileSystemStorage = fileSystemStorage.GetNextStorage();
            count++;
        }
        Assert.Equal(depthCount, count);
    }
     [Fact]
        public void StorageInChainNotReadOnly()
        {
            IStorage<JsonDumb> fileSystemStorage = new FileSystemStorage(depthCount, expirationInterval);
            while (fileSystemStorage.GetNextStorage() != null)
            {
                Assert.False(fileSystemStorage.IsReadOnly());
                fileSystemStorage = fileSystemStorage.GetNextStorage();
            }
            Assert.True(fileSystemStorage.IsReadOnly());
        }

        [Fact]
        public void GetNotValidValue()
        {
            string notValidKey = "NONVALIDKEY";
            Assert.Throws<InvalidOperationException>(() =>
            {
                IStorage<JsonDumb> fileSystemStorage = new FileSystemStorage(depthCount, expirationInterval);
                fileSystemStorage.GetValue(notValidKey);
            });
        }

        [Fact]
        public void GetValueNoDepth()
        {
            string validKey = "VALIDKEY";
            JsonDumb jsonDumb = new JsonDumb("DumbName", 16);
            IStorage<JsonDumb> fileSystemStorage = new FileSystemStorage(depthCount, expirationInterval);
            fileSystemStorage.AddValue(validKey, jsonDumb);
            JsonDumb returnJsonDumb = fileSystemStorage.GetValue(validKey);
            Assert.Equal(jsonDumb.Name, returnJsonDumb.Name);
            Assert.Equal(jsonDumb.Age, returnJsonDumb.Age);
        }

        [Fact]
        public void GetValueWithDepth()
        {
            string validKey = "VALIDKEY";
            JsonDumb jsonDumb = new JsonDumb("DumbName", 16);
            int validDepth = 5;
            IStorage<JsonDumb> fileSystemStorage = new FileSystemStorage(depthCount, expirationInterval);
            fileSystemStorage.AddValue(validKey, jsonDumb, validDepth);
            JsonDumb returnJsonDumb = fileSystemStorage.GetValue(validKey);
            Assert.Equal(jsonDumb.Name, returnJsonDumb.Name);
            Assert.Equal(jsonDumb.Age, returnJsonDumb.Age);
        }

        [Fact]
        public void GetValueWithIllegalDepth()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                string validKey = "VALIDKEY";
                JsonDumb jsonDumb = new JsonDumb("DumbName", 16);
                int invalidDepth = 6;
                IStorage<JsonDumb> fileSystemStorage = new FileSystemStorage(depthCount, expirationInterval);
                fileSystemStorage.AddValue(validKey, jsonDumb, invalidDepth);
            });
        }

        [Fact]
        public void CheckPropagate()
        {
            string validKey = "VALIDKEY";
            JsonDumb jsonDumb = new JsonDumb("DumbName", 16);
            int validDepth = 3;
            IStorage<JsonDumb> fileSystemStorage = new FileSystemStorage(depthCount, expirationInterval);
            fileSystemStorage.AddValue(validKey, jsonDumb, validDepth);
            fileSystemStorage.GetValue(validKey);
            IStorage<JsonDumb> currentStorage = fileSystemStorage;
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
