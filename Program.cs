using Entities;
using Interfaces;
using Storages;

class Program
{
    static void Main(string[] args){
         int depthCount = 5;
            int memoryExpirationInterval = 1;
            int fileSystemExpirationInterval = 4;
            int webServiceExpirationInterval = 0;

            Console.WriteLine("Helloo");

            IStorage<string> memoryStorage = new MemoryStorage(depthCount, memoryExpirationInterval);
            IStorage<JsonDumb> fileSystemStorage = new FileSystemStorage(depthCount, fileSystemExpirationInterval);
            IStorage<string> webServiceStorage = new WebServiceStorage(depthCount, webServiceExpirationInterval);
    }  
}