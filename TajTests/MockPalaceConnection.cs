using System;
using MiscUtil.IO;
using Taj;

namespace TajTests
{
    internal class MockPalaceConnection : IPalaceConnection
    {
        #region IPalaceConnection Members

        public PalaceUser Identity
        {
            get { throw new NotImplementedException(); }
        }

        public Palace Palace
        {
            get { throw new NotImplementedException(); }
        }

        public EndianBinaryReader Reader { get; set; }

        public EndianBinaryWriter Writer { get; set; }

        #endregion
    }
}