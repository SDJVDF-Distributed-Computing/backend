namespace Backend.Infrastructure.Smp.Protocol;

public sealed record SMPRawResponse(SMPResponseCode Code, string Message);
