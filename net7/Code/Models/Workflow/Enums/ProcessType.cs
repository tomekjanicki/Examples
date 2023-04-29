namespace Code.Models.Workflow.Enums;

public enum ProcessType : byte
{
    ReportDataProvider,
    Enrichment,
    Validation,
    HotStorePublisher,
    ColdStorePublisher,
    ColdStoreLoader
}