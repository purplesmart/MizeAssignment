using Interfaces;
using Newtonsoft.Json.Linq;

namespace Storages;
    public class WebServiceStorage : StorageInChainBase<string>
    {
        const string RATES_PROPERTY_NAME = "rates";
        string AppId;

        public WebServiceStorage(int depth, int expirationInterval, bool viaClone= false) : base(0, -1, viaClone)
        {
            Init();
        }

        public override void Init()
        {
            AppId = "0cf998325ef741da8f428054d6c846b6";
        }

        public override void AddValue(string key, string value)
        {
            throw new InvalidOperationException("The storage doesn't contain AddValue");
        }

        public override bool ValueExists(string key)
        {
            try
            {
                GetValue(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string GetValue(string key)
        {
            Task<string> ratesObject = GetValueFromService();    
            return ratesObject.Result;
        }

        private async Task<string> GetValueFromService(){

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("$https://openexchangerates.org/api/latest.json?app_id={AppId}&symbols={key}");

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        JObject jsonObject = JObject.Parse(jsonString);

                        if(jsonObject.ContainsKey(RATES_PROPERTY_NAME)){
                            return jsonObject[RATES_PROPERTY_NAME].ToObject<string>();
                        } else
                        {
                            throw new KeyNotFoundException("The Coin sign does not exist");
                        }
                    }
                    throw new SystemException("Failed to process reequest");
                }
                catch (Exception e)
                {
                    throw new SystemException("Failed to process reequest");
                }
            }
        }

        public override IStorage<string> Clone()
        {
            return new WebServiceStorage(Depth, ExpirationInterval, true);
        }

        public override IStorage<string> GetStorage()
        {
            return this;
        }
    }

