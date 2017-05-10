using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integreat.Shared.Utilities
{
    public class SprungbrettTemp
    {
        private int _totalOffersCount;

        
        public int TotalOffersCount
        {
            get { return _totalOffersCount; }
            set { _totalOffersCount = value; }
        }

        public Task<List<JobOffer>> GetCareerList()
        {

            throw new NotImplementedException();
        }

        //JobOffer class to save and manipulate the json elements
        public class JobOffer
        {
            
        }
    }
}