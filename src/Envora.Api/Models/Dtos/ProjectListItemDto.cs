namespace Envora.Api.Models.Dtos;

public sealed record ProjectListItemDto(
    Guid ProjectId,
    string ProjectNumber,
    string ProjectName,
    string Status,
    Guid? CustomerId,
    Guid? EngineeringFirmId,
    DateOnly? StartDate,
    DateOnly? EstimatedCompletion,
    decimal? BudgetAmount
);


