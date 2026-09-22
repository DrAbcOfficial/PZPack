using System.Drawing;
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

        private static PZPage MakePage(string name, string entryName)
        {
            return new PZPage
            {
                Name = name,
                Mask = 1,
                Png = TestData.MinimalPng,
                Entries = [
                    new PZEntry
                    {
                        Name = entryName,
                        Position = new Point(0, 0),
                        Size = new Size(1, 1),
                        Offset = new Size(0, 0),
                        TotalSize = new Size(1, 1),
                    }
                ],
            };
        }

        private static void Roundtrip(IPZPack pack, IPZPack.PZPackType expectedType)
        {
            string outputFile = Path.GetTempFileName();
            try
            {
                using (FileStream fs = new(outputFile, FileMode.Create, FileAccess.Write))
                {
                    pack.Encode(fs);
                }

                Assert.AreEqual(expectedType, PZPack.IsFileAPZPack(outputFile));

                using FileStream read = new(outputFile, FileMode.Open, FileAccess.Read);
                PZPack reparsed = expectedType == IPZPack.PZPackType.V1
                    ? PZPack.OpenV1(outputFile)
                    : PZPack.OpenV2(outputFile);

                Assert.HasCount(pack.Pages.Length, reparsed.Pages);
                for (int i = 0; i < pack.Pages.Length; i++)
                {
                    PZPage original = pack.Pages[i];
                    PZPage page = reparsed.Pages[i];
                    Assert.AreEqual(original.Name, page.Name);
                    Assert.AreEqual(original.Mask, page.Mask);
                    Assert.HasCount(original.Entries.Length, page.Entries);
                    Assert.AreEqual(original.Entries.First().Name, page.Entries.First().Name);
                    CollectionAssert.AreEqual(original.Png, page.Png);
                }
            }
            finally
            {
                File.Delete(outputFile);
            }
        }

        [TestMethod]
        public void TestV1_MultiPageRoundtrip()
        {
            PZPackV1 pzpk = new()
            {
                Pages = [MakePage("pageA", "spriteA"), MakePage("pageB", "spriteB")],
            };
            Roundtrip(pzpk, IPZPack.PZPackType.V1);
        }

        [TestMethod]
        public void TestV2_MultiPageRoundtrip()
        {
            PZPackV2 pzpk = new()
            {
                Mask = 1,
                Pages = [MakePage("pageA", "spriteA"), MakePage("pageB", "spriteB")],
            };
            Roundtrip(pzpk, IPZPack.PZPackType.V2);
        }
    }
}
