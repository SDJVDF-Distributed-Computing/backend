namespace Backend.Application.Auth.DTOs;

public sealed record SessionStatusDTO(bool IsConnected, bool IsAuthenticated, bool IsClosed);
