using System.Diagnostics;
using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces;

public interface IFlatAdsService
{
    public Task<double> FloorToPricePerSquareMeterCorrelationAsync(double priceThreshold, CancellationToken cancellationToken); 
    public Task<double> RoomsToPricePerSquareMeterCorrelationAsync(double priceThreshold, CancellationToken cancellationToken);
    public Task<double> MetroStationToPricePerSquareMeterCorrelationAsync(double priceThreshold, CancellationToken cancellationToken);
    public Task<CorrelationCollectionResponseDTO> GetCorrelationsAsync(double priceThreshold, CancellationToken cancellationToken);
}