using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTool.Model
{
    public class Tool
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Tags { get; set; }
    }
}
