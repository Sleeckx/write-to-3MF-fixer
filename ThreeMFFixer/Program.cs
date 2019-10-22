using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeMFFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            var enUs = new CultureInfo("en-US");
            using (var zipToOpen = new FileStream(@"<<FULL_FILEPATH_HERE>>", FileMode.Open))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    var modelEntry = archive.GetEntry("3D/3dmodel.model");

                    var xml = new HtmlDocument();
                    using (var modelStream = modelEntry.Open())
                    {
                        xml.Load(modelStream);

                        foreach (var vertex in xml.DocumentNode.SelectNodes("//vertex"))
                        {
                            var x = Convert.ToDecimal(vertex.GetAttributeValue("x", null), enUs) * 10;
                            var y = Convert.ToDecimal(vertex.GetAttributeValue("y", null), enUs) * 10;
                            var z = Convert.ToDecimal(vertex.GetAttributeValue("z", null), enUs) * 10;

                            vertex.SetAttributeValue("x", x.ToString(enUs));
                            vertex.SetAttributeValue("y", y.ToString(enUs));
                            vertex.SetAttributeValue("z", z.ToString(enUs));
                        }
                    }

                    using (var newModelStream = modelEntry.Open())
                    {
                        xml.Save(newModelStream);
                    }
                }
            }
        }
    }
}