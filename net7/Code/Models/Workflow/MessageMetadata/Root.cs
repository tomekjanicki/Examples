using Code.Types.Collections;

namespace Code.Models.Workflow.MessageMetadata;

public sealed record Root(ReportDataProviderMessage? ReportDataProviderMessage, EnrichmentMessage? EnrichmentMessage,
    ValidationMessage? ValidationMessage, ReadOnlyArrayWithIsEquivalent<int> ReportDataProviderMessageCallIds);