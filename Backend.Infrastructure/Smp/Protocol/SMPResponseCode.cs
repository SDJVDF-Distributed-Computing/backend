namespace Backend.Infrastructure.Smp.Protocol;

public enum SMPResponseCode
{
    Ok          = 200,
    Challenge   = 201,
    Message     = 202,
    EndMessages = 203,
    BadRequest  = 400,
    Unauthorized = 401,
    Forbidden   = 403,
    ServerError = 500
}
