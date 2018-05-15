using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CorePract.Dto;
using CorePract.Dto.Enum;
using System.Linq;

namespace CorePract.Services
{
    public class IssuesStorage
    {
        private List<IssueDto> _issues;

        public IssuesStorage()
        {
            _issues = new List<IssueDto>();
        }
    
        public ReadOnlyCollection<IssueDto> GetIssues()
        {
            return new ReadOnlyCollection<IssueDto>(_issues);
        }

        public bool isEmpty()
        {
            if(_issues.Count==0)
            {
                return true;
            }
            return false;
        }

        public bool hasIssue( string id )
        {
            return _issues.Any(issue => issue.IssueId == id);
        }

        public IssueDto CreateIssue( IssueRawDto issueRaw )
        {
            IssueDto issue = new IssueDto();

            issue.IssueId = Guid.NewGuid().ToString();
            issue.status = IssuesStatus.New;

            issue.Sum = issueRaw.Sum;
            issue.SenderId = issueRaw.SenderId;
            issue.ReceiverId = issueRaw.ReceiverId;

            return issue;
        }

        public void UpdateStatus( string id , IssuesStatus newStatus )
        {
            _issues.First(issue => id == issue.IssueId).status = newStatus;

        }

        public void Insert(IssueDto issue)
        {
            if(!hasIssue(issue.IssueId))
            {
                _issues.Add(issue);
            }
        }

        public IssueDto GetIssueById(string id)
        {
           return  _issues.First(issue => id == issue.IssueId);
        }

        


    }
}
