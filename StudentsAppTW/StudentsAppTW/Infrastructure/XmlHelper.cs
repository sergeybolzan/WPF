using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsAppTW.Infrastructure
{
    internal static class XmlHelper
    {
        public static T XmlDeserialize<T>(string fileName) where T : new()
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException();


            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                try
                {
                    return (T)xml.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    throw new XmlHelperException("Ошибка в структуре файла", ex);
                }
            }
        }
    }
}
