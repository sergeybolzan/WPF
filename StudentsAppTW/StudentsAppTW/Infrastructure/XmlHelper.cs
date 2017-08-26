using StudentsAppTW.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace StudentsAppTW.Infrastructure
{
    internal static class XmlHelper
    {
        /// <summary>
        /// Deserializing xml to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string fileName) where T : new()
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }

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

        /// <summary>
        /// Adding new record to XML.
        /// </summary>
        /// <param name="student"></param>
        public static void AddNewStudentToXml(Student student)
        {
            string fileName = "Students.xml";
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }

            try
            {
                var xDocument = XDocument.Load(fileName);
                xDocument.Element("Students").Add(
                    new XElement("Student",
                    new XAttribute("Id", student.Id),
                    new XElement("FirstName", student.FirstName),
                    new XElement("Last", student.Last),
                    new XElement("Age", student.Age),
                    new XElement("Gender", Convert.ToInt32(student.Gender))));
                xDocument.Save(fileName);
            }
            catch (Exception ex)
            {
                throw new XmlHelperException("Ошибка в структуре файла", ex);
            }
        }

        /// <summary>
        /// Removing record(s) from XML.
        /// </summary>
        /// <param name="students"></param>
        public static void RemoveStudentsFromXml(IList students)
        {
            string fileName = "Students.xml";
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }

            try
            {
                var xDocument = XDocument.Load(fileName);
                for (int i = 0; i < students.Count; i++)
                {
                    var student = students[i] as Student;
                    var xElementToRemove = from XElement in xDocument.Elements("Students").Elements("Student")
                                           where XElement.Attribute("Id").Value == XmlConvert.ToString(student.Id)
                                           select XElement;
                    xElementToRemove.Remove();
                }
                xDocument.Save(fileName);
            }
            catch (Exception ex)
            {
                throw new XmlHelperException("Ошибка в структуре файла", ex);
            }
        }

        /// <summary>
        /// Editing record in XML.
        /// </summary>
        /// <param name="student"></param>
        public static void EditStudentInXml(Student student)
        {
            string fileName = "Students.xml";
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }

            try
            {
                var xDocument = XDocument.Load(fileName);
                foreach (var xElement in xDocument.Root.Elements())
                {
                    if (Convert.ToInt32(xElement.Attribute("Id").Value) == student.Id)
                    {
                        xElement.Element("FirstName").Value = student.FirstName;
                        xElement.Element("Last").Value = student.Last;
                        xElement.Element("Age").Value = student.Age.ToString();
                        xElement.Element("Gender").Value = Convert.ToInt32(student.Gender).ToString();
                    }
                }
                xDocument.Save(fileName);
            }
            catch (Exception ex)
            {
                throw new XmlHelperException("Ошибка в структуре файла", ex);
            }

        }
    }
}
