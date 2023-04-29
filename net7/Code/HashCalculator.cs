using System.Security.Cryptography;
using Code.Types.Collections;

namespace Code;

public static class HashCalculator
{
    public static NonEmptyReadOnlyArray<byte> GetHash(DateTime startDate, DateTime endDate)
    {
        var counter = 0;
        Span<byte> dataSpan = stackalloc byte[16];
        Write(startDate, dataSpan, ref counter);
        Write(endDate, dataSpan, ref counter);
        Span<byte> hashSpan = stackalloc byte[32];
        if (!TryComputeSha256Hash(dataSpan, hashSpan, out var bytes))
        {
            RaiseSpanCopyFailureException();
        }

        return NonEmptyReadOnlyArray<byte>.TryCreate(hashSpan[..bytes].ToArray()).AsT0;
    }

    private static void RaiseSpanCopyFailureException() => throw new InvalidOperationException("Failed to copy source to span");

    private static void Write(DateTime value, Span<byte> span, ref int counter)
    {
        const int length = 8;
        if (!BitConverter.TryWriteBytes(span[..length], value.Ticks))
        {
            RaiseSpanCopyFailureException();
        }
        counter += length;
    }

    private static bool TryComputeSha256Hash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten) => SHA256.TryHashData(source, destination, out bytesWritten);
}