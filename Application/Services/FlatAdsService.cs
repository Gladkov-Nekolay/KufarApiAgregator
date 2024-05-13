using Core.DTOs;
using Core.Interfaces;
using MathNet.Numerics.Statistics;

namespace Application.Services;

public class FlatAdsService : IFlatAdsService
{
    private readonly IKufarApiAccessor _kufarApiAccessor;

    public FlatAdsService(IKufarApiAccessor kufarApiAccessor)
    {
        _kufarApiAccessor = kufarApiAccessor;
    }
    public async Task<double> FloorToPricePerSquareMeterCorrelationAsync(double priceThreshold, CancellationToken cancellationToken)
    {
        var result = await _kufarApiAccessor.GetFlatAdsAsync(cancellationToken);

        var thresholdResult = result.Where(x => x.Price > priceThreshold).ToArray();

        var floors = thresholdResult.Select(x => (double)x.Floor).ToArray();
        var pricesPerSquareMeter = result.Select(x => x.PricePerSquareMeter).ToArray();

        double correlation = Correlation.Pearson(floors, pricesPerSquareMeter);
        return correlation;
    }

    public async Task<double> RoomsToPricePerSquareMeterCorrelationAsync(double priceThreshold, CancellationToken cancellationToken)
    {
        var result = await _kufarApiAccessor.GetFlatAdsAsync(cancellationToken);

        var thresholdResult = result.Where(x => x.Price > priceThreshold).ToArray();

        var rooms = thresholdResult.Select(x => (double)x.Rooms).ToArray();
        var pricesPerSquareMeter = result.Select(x => x.PricePerSquareMeter).ToArray();

        double correlation = Correlation.Pearson(rooms, pricesPerSquareMeter);
        return correlation;
    }

    public async Task<double> MetroStationToPricePerSquareMeterCorrelationAsync(double priceThreshold, CancellationToken cancellationToken)
    {
        var result = await _kufarApiAccessor.GetFlatAdsAsync(cancellationToken);

        var thresholdResult = result.Where(x => x.Price > priceThreshold).ToArray();

        var metroThresholdResult = thresholdResult.Where(x => x.MetroStation != null).ToArray();

        var metro = metroThresholdResult.Select(x => (double)x.MetroStation).ToArray();
        var pricesPerSquareMeter = metroThresholdResult.Select(x => x.PricePerSquareMeter).ToArray();

        double correlation = Correlation.Pearson(metro, pricesPerSquareMeter);

        return correlation;
    }

    public async Task<CorrelationCollectionResponseDTO> GetCorrelationsAsync(double priceThreshold, CancellationToken cancellationToken)
    {
        var correlationCollectionResponseDto = new CorrelationCollectionResponseDTO();

        correlationCollectionResponseDto.CorrelationCollection.Add(new CorrelationRespnseDTO()
        {
            CorrelationFeatureNames = "Floor/Price Per Square Meter", 
            Value = await FloorToPricePerSquareMeterCorrelationAsync(priceThreshold, cancellationToken)
        });

        correlationCollectionResponseDto.CorrelationCollection.Add(new CorrelationRespnseDTO()
        {
            CorrelationFeatureNames = "Rooms/Price Per Square Meter",
            Value = await RoomsToPricePerSquareMeterCorrelationAsync(priceThreshold, cancellationToken)
        });

        correlationCollectionResponseDto.CorrelationCollection.Add(new CorrelationRespnseDTO()
        {
            CorrelationFeatureNames = "Metro/Price Per Square Meter",
            Value = await FloorToPricePerSquareMeterCorrelationAsync(priceThreshold, cancellationToken)
        });

        return correlationCollectionResponseDto;
    }
}