using MiscUtil.IO;

namespace Taj
{
    public interface IPalaceConnection
    {
        PalaceUser Identity { get; }
        Palace Palace { get; }
        EndianBinaryReader Reader { get; }
        EndianBinaryWriter Writer { get; }
    }
}