/*
 * From Jon Skeet's MiscUtil library http://www.yoda.arachsys.com/csharp/miscutil/
 */

namespace Ethereum.Utilities
{
    /// <summary>
    /// Endianness of a converter
    /// </summary>
    public enum Endianness
    {
        /// <summary>
        /// Little endian - least significant byte first
        /// </summary>
        LittleEndian,
        /// <summary>
        /// Big endian - most significant byte first
        /// </summary>
        BigEndian
    }
}
