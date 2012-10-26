using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages.Flags
{
    /// <summary>
    /// ulDownloadCaps indicates the client’s capabilities with respect to downloading assets and files
    /// </summary>
    [Flags]
    public enum ulDownloadCapsFlags : uint
    {
        ASSETS_PALACE     = 0x00000001,
        ASSETS_FTP        = 0x00000002,
        ASSETS_HTTP       = 0x00000004,
        ASSETS_OTHER      = 0x00000008,
        FILES_PALACE      = 0x00000010,
        FILES_FTP         = 0x00000020,
        FILES_HTTP        = 0x00000040,
        FILES_OTHER       = 0x00000080,
        FILES_HTTPSrvr    = 0x00000100, //The only bit which the Unix server examines is LI_DLCAPS_FILES_HTTPSrvr.
        EXTEND_PKT        = 0x00000200,
    }
}
