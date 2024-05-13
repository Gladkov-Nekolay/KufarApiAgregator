namespace Core.DTOs;

public class CorrelationCollectionResponseDTO
{
    public List<CorrelationRespnseDTO> CorrelationCollection { get; set; } = new List<CorrelationRespnseDTO>();
}

public class CorrelationRespnseDTO
{
    public string CorrelationFeatureNames { get; set; }
    public double Value { get; set; }
}