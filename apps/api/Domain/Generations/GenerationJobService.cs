namespace Drakoda.Api.Domain.Generations;

public interface IGenerationJobService
{
    Task ExecuteAsync(Guid generationId, CancellationToken cancellationToken);
}
