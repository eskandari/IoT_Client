using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Common
{
    public class Telemetry
    {
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public decimal Humidity { get; set; }

        public StatusType Status { get; set; }
    }
}
