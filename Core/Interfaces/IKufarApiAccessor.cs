using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IKufarApiAccessor
    {
        public Task<List<FlatAds>?> GetFlatAdsAsync(CancellationToken cancellationToken);
        public Task<List<FlatRentAds>> GetRentFlatAdsAsync(CancellationToken cancellationToken);
    }
}
