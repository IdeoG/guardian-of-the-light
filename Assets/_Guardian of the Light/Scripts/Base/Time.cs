using System;
using System.Globalization;

public static class MTime
{
    public static string Now => DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
}