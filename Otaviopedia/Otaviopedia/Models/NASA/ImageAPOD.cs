using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otaviopedia.Models.NASA
{
    public class ImageAPOD
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string MediaType { get; set; }
        public string Copyright { get; set; }
        public string SourceAPI { get; set; }
    }
}

