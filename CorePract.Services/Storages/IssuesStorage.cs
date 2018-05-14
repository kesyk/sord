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

        public void UpdateRawIssue( ref IssueDto issue )
        {
            issue.IssueId = Guid.NewGuid().ToString();
            issue.status = IssuesStatus.New;
        }

        public void UpdateStatus( string id , IssuesStatus newStatus )
        {
            _issues.First(issue => id == issue.IssueId).status = newStatus;

        }

        public void Insert(IssueDto issue)
        {
            _issues.Add(issue);
        }

        public IssueDto GetIssueById(string id)
        {
           return  _issues.First(issue => id == issue.IssueId);
        }

        


    }
}
