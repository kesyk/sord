using System;
using System.Collections.Generic;
using System.Text;
using CorePract.Dto;

namespace CorePract.Controllers.Validators
{
    public class RegIssueParamsValidator
    {
        public RegIssueParamsValidator() { }

        public bool ValidateQueryParams( IssueDto issue )
        {
            try
            {
                if (RequiredStringValueIsEmpty(issue.IssueId) ||
                                RequiredStringValueIsEmpty(issue.ReceiverId) ||
                                issue.Sum < 0 ||
                                RequiredStringValueIsEmpty(issue.Instrument))
                    return true;

                return false;
            }
            catch(Exception)
            {
                return false;
            }
            
        }
        

        private bool RequiredStringValueIsEmpty( string value )
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

    }
}
