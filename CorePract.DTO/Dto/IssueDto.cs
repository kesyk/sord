using System;
using CorePract.Dto.Enum;

namespace CorePract.Dto
{
    public class IssueDto
    {
        public string IssueId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public IssuesStatus status;
        public  decimal Sum { get; set; }
        public  string Instrument { get; set; }
    }
}
