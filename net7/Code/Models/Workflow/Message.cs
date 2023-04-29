using Code.Models.Workflow.Entities;
using Code.Models.Workflow.Entities.Abstractions;
using Code.Models.Workflow.Enums;
using Code.Types.Collections;

namespace Code.Models.Workflow;

public sealed record Message(MessageId Id, MessageType Type, ReadOnlyDictionaryWithEquality<ItemKey, MetadataValue> Metadata);