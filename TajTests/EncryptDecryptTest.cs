using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiscUtil.Conversion;
using MiscUtil.IO;
using Taj;
using Taj.Messages;
using Taj.Messages.Structures;

namespace TajTests
{
    [TestClass]
    public class EncryptDecryptTest
    {
        private const string testMsg = "The quick brown fox jumps over the lazy dog.";

        [TestMethod]
        public void TestEncryptDecrypt()
        {
            byte[] cryptoBytes = PalaceEncryption.Encrypt(testMsg);
            string decryptMsg = PalaceEncryption.Decrypt(cryptoBytes);
            Assert.AreEqual(testMsg, decryptMsg);
        }

        [TestMethod]
        public void Test_MH_XTalk_EncryptDecrypt()
        {
            using (var testMemStream = new MemoryStream())
            {
                using (var writer = new EndianBinaryWriter(new LittleEndianBitConverter(), testMemStream))
                {
                    using (var reader = new EndianBinaryReader(new LittleEndianBitConverter(), testMemStream))
                    {
                        var mockCon = new MockPalaceConnection {Reader = reader, Writer = writer};

                        var mhxtalk = new MH_XTalk(mockCon, testMsg);
                        Assert.AreEqual(testMsg, mhxtalk.Text);
                        mhxtalk.Write();

                        //Console.WriteLine(testMemStream.ToArray());

                        testMemStream.Seek(0, SeekOrigin.Begin);
                        var cmsg = reader.ReadStruct<ClientMessage>();
                        var read_mhxtalk = new MH_XTalk(mockCon, cmsg);

                        Assert.AreEqual(mhxtalk.Text, read_mhxtalk.Text);
                    }
                }
            }
        }
    }
}