using MiscUtil.IO;
using Taj.Assets;

namespace Taj
{
    public interface IPalaceConnection
    {
        PalaceIdentity Identity { get; }
        Palace Palace { get; }
        IAssetManager AssetStore { get; }
        EndianBinaryReader Reader { get; }
        EndianBinaryWriter Writer { get; }
    }
}