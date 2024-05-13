using Newtonsoft.Json;

namespace Core.DTOs;

public class ApiResponseDTO
{
    [JsonProperty("ads")]
    public FlatAdsKufarApiDTO[] Advertisements { get; set; } 
}

public class FlatAdsKufarApiDTO
{
    [JsonProperty("ad_parameters")]
    public AdPrameters[] AdPrameters { get; set; }
    [JsonProperty("price_usd")]
    public object Price { get; set; }
}

public class AdPrameters
{
    [JsonProperty("p")]
    public string ParameterName { get; set; }

    [JsonProperty("v")]
    public object Value { get; set; }
}