using Core.Configurations;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application
{
    public class KufarApiAccessor: IKufarApiAccessor
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly IOptions<KufarApiRequestConfiguration> _kufarOptions;
        private readonly IOptions<CacheConfiguration> _cacheConfiguration;

        public KufarApiAccessor(IHttpClientFactory httpClientFactory, 
            IMemoryCache cache, 
            IOptions<KufarApiRequestConfiguration> kufarOptions, 
            IOptions<CacheConfiguration> cacheConfiguration)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _kufarOptions = kufarOptions;
            _cacheConfiguration = cacheConfiguration;
        }

        public async Task<List<FlatAds>> GetFlatAdsAsync(CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue("FlatAdsList", out List<FlatAds>? result))
            {
                return result ?? new List<FlatAds>();
            }

            result = new List<FlatAds>();

            var kufarApiResponse = await GetFlatAdsKufarApiDTOAsync(cancellationToken);

            foreach (var flatAdsKufarApiDTO in kufarApiResponse)
            {
                var flatAd = new FlatAds();
                flatAd.Price = int.Parse(flatAdsKufarApiDTO.Price.ToString());

                foreach (var AdPrameter in flatAdsKufarApiDTO.AdPrameters)
                {
                    switch (AdPrameter.ParameterName)
                    {
                        case "size":
                        {
                            flatAd.Size = double.Parse(AdPrameter.Value.ToString());
                            break;
                        }
                        case "floor":
                        {
                            int[] floor = ((JArray)AdPrameter.Value).ToObject<int[]>();
                            flatAd.Floor = floor[0];
                            break;
                        }
                        case "rooms":
                        {
                            flatAd.Rooms = int.Parse(AdPrameter.Value.ToString());
                            break;
                        }
                        case "square_meter":
                        {
                            flatAd.PricePerSquareMeter = double.Parse(AdPrameter.Value.ToString());
                            break;
                        }
                        case "coordinates":
                        {
                            flatAd.Coordinates = ((JArray)AdPrameter.Value).ToObject<double[]>();
                            break;
                        }
                        case "metro":
                        {
                            int[] metro = ((JArray)AdPrameter.Value).ToObject<int[]>();
                            flatAd.MetroStation = metro[0];
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }

                result.Add(flatAd);
            }

            _cache.Set("FlatAdsList", result, 
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(
                    _cacheConfiguration.Value.CacheLifeTimeMinutes)));

            return result;
        }

        public async Task<FlatAdsKufarApiDTO[]> GetFlatAdsKufarApiDTOAsync(CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient();

            using (var response = await httpClient.GetAsync(_kufarOptions.Value.KufarURL,
                       HttpCompletionOption.ResponseHeadersRead, 
                       cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                var apiResponse = JsonConvert.DeserializeObject<ApiResponseDTO>(content);

                return apiResponse.Advertisements;
            }
        }
    }
}