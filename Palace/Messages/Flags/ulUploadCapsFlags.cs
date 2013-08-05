using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palace.Messages.Flags
{
    /// <summary>
    /// ulUploadCaps indicates the client’s capabilities with respect to uploading assets and files
    /// </summary>
    [Flags]
    public enum ulUploadCapsFlags : uint 
    {
        ASSETS_PALACE   = 0x00000001,
        ASSETS_FTP      = 0x00000002,
        ASSETS_HTTP     = 0x00000004,
        ASSETS_OTHER    = 0x00000008,
        FILES_PALACE    = 0x00000010,
        FILES_FTP       = 0x00000020,
        FILES_HTTP      = 0x00000040,
        FILES_OTHER     = 0x00000080,
        EXTEND_PKT      = 0x00000100,
    }
}
