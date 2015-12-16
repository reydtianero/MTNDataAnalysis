using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTNDataAnalysis.Models
{
    public class CallDataSummary
    {
        public string  BillingPeriod { get; set; }
        
        public string GroupingField { get; set; }

        public string HumanReadableSum { get; set; }

        public long SumInBytes { get; set; }
         
    }
}
