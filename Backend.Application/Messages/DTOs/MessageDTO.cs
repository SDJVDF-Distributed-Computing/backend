namespace Backend.Application.Messages.DTOs;

public sealed record MessageDTO(Guid Id, string Content, DateTime ReceivedAt);
