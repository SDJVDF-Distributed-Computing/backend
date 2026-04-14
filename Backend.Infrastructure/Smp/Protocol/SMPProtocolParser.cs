namespace Backend.Infrastructure.Smp.Protocol;

public static class SMPProtocolParser
{
    public static SMPRawResponse ParseLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new FormatException("Received empty response from server.");

        var spaceIndex = line.IndexOf(' ');
        var codePart = spaceIndex >= 0 ? line[..spaceIndex] : line;
        var messagePart = spaceIndex >= 0 ? line[(spaceIndex + 1)..] : string.Empty;

        if (!int.TryParse(codePart, out var codeInt) || !Enum.IsDefined(typeof(SMPResponseCode), codeInt))
            throw new FormatException($"Unrecognised SMP response code: '{codePart}'");

        return new SMPRawResponse((SMPResponseCode)codeInt, messagePart);
    }

    public static string BuildCommand(string verb, string? arg = null) =>
        arg is null ? $"{verb}\n" : $"{verb} {arg}\n";
}
