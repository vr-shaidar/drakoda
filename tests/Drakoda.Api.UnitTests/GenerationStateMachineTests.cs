using Drakoda.AI;
using Drakoda.Api.Domain.Generations;

namespace Drakoda.Api.UnitTests;

public sealed class GenerationStateMachineTests
{
    [Fact]
    public void Validating_can_transition_to_moderation()
    {
        var generation = New(GenerationStatus.Validating);
        GenerationStateMachine.Transition(generation, GenerationStatus.Moderation);
        Assert.Equal(GenerationStatus.Moderation, generation.Status);
    }

    [Fact]
    public void Processing_cannot_transition_back_to_queued()
    {
        var generation = New(GenerationStatus.Processing);
        Assert.Throws<InvalidOperationException>(() => GenerationStateMachine.Transition(generation, GenerationStatus.Queued));
    }

    private static Generation New(GenerationStatus status) => new() { Id = Guid.NewGuid(), Status = status, Prompt = "test", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow };
}
