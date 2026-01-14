using PZPack.Interface;

namespace PZPack.Test
{
    [TestClass]
    public sealed class Test1
    {
        private string _tempFile = string.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            _tempFile = Path.GetTempFileName();
            File.WriteAllBytes(_tempFile, TestData.PZPackV2);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(_tempFile))
            {
                File.Delete(_tempFile);
            }
        }

        [TestMethod]
        public void TestIsFileAPZPack()
        {
            IPZPack.PZPackType type = PZPack.IsFileAPZPack(_tempFile);
            Assert.AreEqual(IPZPack.PZPackType.V2, type);
        }

        [TestMethod]
        public void TestPZPackV2()
        {
            PZPackV2 pzpk = PZPack.OpenV2(_tempFile);
            Assert.AreEqual(1, pzpk.Mask);
            Assert.HasCount(1, pzpk.Pages);
            Assert.AreEqual("LS_Artwork20", pzpk.Pages.First().Name);

            Assert.HasCount(2, pzpk.Pages.First().Entries);
        }

        [TestMethod]
        public void TestPZPackV2_EntriesDetails()
        {
            PZPackV2 pzpk = PZPack.OpenV2(_tempFile);
            var entries = pzpk.Pages.First().Entries;

            Assert.IsNotNull(entries);
            Assert.HasCount(2, entries);

            var firstEntry = entries.First();
            Assert.IsNotNull(firstEntry);
            Assert.AreEqual("LS_Inventions_0", firstEntry.Name);

            var secondEntry = entries.Skip(1).First();
            Assert.IsNotNull(secondEntry);
            Assert.AreEqual("LS_Inventions_1", secondEntry.Name);
        }

        [TestMethod]
        public void TestIsFileAPZPack_InvalidFile()
        {
            string invalidFile = Path.GetTempFileName();
            File.WriteAllText(invalidFile, "This is not a PZPack file");

            try
            {
                IPZPack.PZPackType type = PZPack.IsFileAPZPack(invalidFile);
                Assert.AreEqual(IPZPack.PZPackType.NotAPZPack, type);
            }
            finally
            {
                File.Delete(invalidFile);
            }
        }

        [TestMethod]
        public void TestEncodePZPack()
        {
            PZPackV2 pzpk = PZPack.OpenV2(_tempFile);
            string outputFile = Path.GetTempFileName();

            try
            {
                using FileStream fs = new(outputFile, FileMode.Create, FileAccess.Write);
                pzpk.Encode(fs);
            }
            finally
            {
                File.Delete(outputFile);
            }
        }

        [TestMethod]
        public void TestPZPackV2_MagicCode()
        {
            PZPackV2 pzpk = PZPack.OpenV2(_tempFile);
            Assert.AreEqual(IPZPackV2.PZ_PACKV2_MAGIC, pzpk.Magic);
        }
    }
}
