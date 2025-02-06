using System.Diagnostics;
using System.IO;
using System.IO.Compression;


namespace FractalFactory.Database
{
    // Much thanks to https://gist.github.com/GoSato/aff1ffd60e0cf2bb3db7615e56ce6c9a
    public class ByteConverter
    {
        /// <summary>Compress using deflate.</summary>
        /// <returns>The byte compress.</returns>
        /// <param name="source">Source.</param>
        public static byte[] ConvertByteCompress(in byte[] source)
        {
            using (MemoryStream ms = new MemoryStream())
            using (DeflateStream compressedDStream = new DeflateStream(ms, CompressionMode.Compress, true))
            {
                compressedDStream.Write(source, 0, source.Length);

                compressedDStream.Close();

                byte[] destination = ms.ToArray();

                // Debug.WriteLine(source.Length.ToString() + " vs " + ms.Length.ToString());

                return destination;
            }
        }

        /// <summary>Decompress using deflate.</summary>
        /// <returns>The byte decompress.</returns>
        /// <param name="source">Source.</param>
        public static byte[] ConvertByteDecompress(in byte[] source)
        {
            using (MemoryStream input = new MemoryStream(source))
            using (MemoryStream output = new MemoryStream())
            using (DeflateStream decompressedDstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                decompressedDstream.CopyTo(output);

                byte[] destination = output.ToArray();

                // Debug.WriteLine("Decompress Size : " + output.Length);

                return destination;
            }
        }

        /// <summary>Decompress using deflate.</summary>
        /// <remarks>
        /// Unlike ConvertByteCompress(in byte[] source) which created a new
        /// array, the output of this method is written directy into decompressed[].
        /// And while this may be more memory efficient it is also less safe because
        /// you must ensure decompressed[] is the correct size.
        /// </remarks>
        public static void ConvertByteDecompress(in byte[] source, byte[] decompressed)
        {
            using (MemoryStream input = new MemoryStream(source))
            using (MemoryStream output = new MemoryStream(decompressed))
            using (DeflateStream decompressedDstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                decompressedDstream.CopyTo(output);
            }
        }
    }
}
