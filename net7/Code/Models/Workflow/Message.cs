using Code.Models.Workflow.Entities;
using Code.Models.Workflow.Enums;
using Code.Models.Workflow.MessageMetadata;

namespace Code.Models.Workflow;

public sealed record Message(MessageId Id, MessageType Type, Root? Metadata);