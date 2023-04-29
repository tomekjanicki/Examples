using Code.Models.Workflow.Enums;
using Code.Types.Collections;

namespace Code;

public static class Configuration
{
    public static readonly NonEmptyReadOnlyDictionary<MessageType, NonEmptyReadOnlyArray<ProcessType>> ProcessTypesForMessageTypes
        = new(new Dictionary<MessageType, NonEmptyReadOnlyArray<ProcessType>>
        {
            { MessageType.ReportDataProviderMessage, new NonEmptyReadOnlyArray<ProcessType>(ProcessType.Enrichment) },
            { MessageType.EnrichmentMessage, new NonEmptyReadOnlyArray<ProcessType>(ProcessType.Validation) },
            { MessageType.ValidationMessage, new NonEmptyReadOnlyArray<ProcessType>(new [] { ProcessType.HotStorePublisher, ProcessType.ColdStorePublisher }) }
        });
}