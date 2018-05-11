using System;

namespace CoreTest.DTO
{
    public class IssueDto
    {
        public string IdIssue = Guid.NewGuid().ToString();
        public string IdSender { get; set; }
        public string IdReciever { get; set; }
        public  decimal Sum { get; set; }
        public  string Instrument { get; set; }
    }
}
