using MiscUtil.IO;
using Palace.Assets;

namespace Palace
{
    public interface IPalaceConnection
    {
        IPalace Palace { get; }
        IAssetManager AssetStore { get; }
        EndianBinaryReader Reader { get; }
        EndianBinaryWriter Writer { get; }
    }
}