using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.ServiceModel.DTO
{
    public class ResourceContent
    {
        public Stream Stream { get; set; }
        public string MimeType { get; set; }
    }
}
