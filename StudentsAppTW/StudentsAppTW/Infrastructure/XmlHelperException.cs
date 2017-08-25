using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsAppTW.Infrastructure
{
    public class XmlHelperException : Exception
    {
        public XmlHelperException()
        {

        }

        public XmlHelperException(string Message) : base(Message)
        {

        }

        public XmlHelperException(string Message, Exception ex) : base(Message, ex)
        {

        }

    }
}
