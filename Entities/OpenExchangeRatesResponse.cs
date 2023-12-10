namespace Entities;
public class OpenExchangeRatesResponse
    {
        private string disclaimer;
        private string license;
        private long timestamp;
        private string baseCurrency;
        private List<double> rates;

        public string GetDisclaimer()
        {
            return disclaimer;
        }

        public void SetDisclaimer(string disclaimer)
        {
            this.disclaimer = disclaimer;
        }

        public string GetLicense()
        {
            return license;
        }

        public void SetLicense(string license)
        {
            this.license = license;
        }

        public long GetTimestamp()
        {
            return timestamp;
        }

        public void SetTimestamp(long timestamp)
        {
            this.timestamp = timestamp;
        }

        public string GetBase()
        {
            return baseCurrency;
        }

        public void SetBase(string baseCurrency)
        {
            this.baseCurrency = baseCurrency;
        }

        public List<double> GetRates()
        {
            return rates;
        }

        public void SetRates(List<double> rates)
        {
            this.rates = rates;
        }
    }
