using Code.Models.Workflow.Result.Enums;
using Code.Types.Collections;

namespace Code.Models.Workflow.Result;

public sealed class SlaData
{
    public required DateTime StartDate { get; init; }

    public required DateTime EndDate { get; init; }

    public required PersistAction Action { get; init; }

    public required NonEmptyReadOnlyArray<byte> Hash { get; init; }
}