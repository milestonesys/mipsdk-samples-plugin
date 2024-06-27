using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoOS.Platform.Data;

namespace SCAudioExport.Client
{
    internal class ExportJob
    {
        internal WAVExporter Exporter { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
