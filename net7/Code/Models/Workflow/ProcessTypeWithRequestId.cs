using Code.Models.Workflow.Entities;
using Code.Models.Workflow.Enums;

namespace Code.Models.Workflow;

public readonly record struct ProcessTypeWithRequestId(ProcessType ProcessType, RequestId RequestId);