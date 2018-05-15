using System;
using System.Collections.Generic;
using System.Text;

namespace CorePract.Dto
{
    public class IssueRawDto
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public decimal Sum { get; set; }
        public string Instrument { get; set; }
    }
}
