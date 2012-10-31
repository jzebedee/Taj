using MiscUtil.IO;

namespace Taj
{
    public interface IPalaceConnection
    {
        PalaceIdentity Identity { get; }
        Palace Palace { get; }
        EndianBinaryReader Reader { get; }
        EndianBinaryWriter Writer { get; }
    }
}