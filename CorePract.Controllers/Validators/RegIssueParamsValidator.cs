using System;
using System.Collections.Generic;
using System.Text;
using CorePract.Dto;

namespace CorePract.Controllers.Validators
{
    public class RegIssueParamsValidator
    {
        public RegIssueParamsValidator() { }

        public bool ValidateQueryParams( IssueRawDto issue )
        {
            try
            {
                if (RequiredStringValueIsEmpty(issue.ReceiverId) ||
                    RequiredStringValueIsEmpty(issue.SenderId) ||
                    issue.Sum < 0 ||
                    RequiredStringValueIsEmpty(issue.Instrument))
                    return false;
          
                return true;
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
