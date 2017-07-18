using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskListApp2.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string StatusValue { get; set; }
        public override string ToString()
        {
            return StatusValue;
        }
    }
}
