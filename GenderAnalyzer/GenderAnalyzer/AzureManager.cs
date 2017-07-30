using GenderAnalyzer.DataModels;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenderAnalyzer
{
    class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<GenderGuesserModel> genderStatsTable;
        private AzureManager()
        {
            this.client = new MobileServiceClient("http://genderguesser.azurewebsites.net");
            this.genderStatsTable = this.client.GetTable<GenderGuesserModel>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task<List<GenderGuesserModel>> getGenderInfo()
        {
            return await this.genderStatsTable.ToListAsync();
        }
    }
}
