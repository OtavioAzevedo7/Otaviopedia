using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otaviopedia.Models.OxfordDictionary
{
    public class Dictionary
    {
        
        public string AudioFile { get; set; }
        public string Definition { get; set; }
        public string ShortDefinition { get; set; }
        public string Etymologies { get; set; }
        public string PhoneticSpelling { get; set; }
        public string Provider { get; set; }
        public string Word { get; set; }
    }
}