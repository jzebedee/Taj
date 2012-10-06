using System;
using Taj.Messages;

namespace Taj
{
    public class Palace
    {
        public Version Version { get; set; }
        public Uri HTTPServer { get; set; }
        public ServerPermissions Permissions { get; set; }
        public string Name { get; set; }
        public int Users { get; set; }
    }
}