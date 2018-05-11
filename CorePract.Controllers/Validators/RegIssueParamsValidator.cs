using System;
using System.Collections.Generic;
using System.Text;
using CoreTest.DTO;

namespace CorePract.Controllers.Validators
{
    class RegIssueParamsValidator
    {
        public RegIssueParamsValidator() { }

        public void ValidateQueryParams( IssueDto issue )
        {
            if (RequiredStringValueIsEmpty(issue.IdSender) ||
                RequiredStringValueIsEmpty(issue.IdReciever) ||
                issue.Sum < 0 ||
                RequiredStringValueIsEmpty(issue.Instrument))
                ThrowErrorValidation("Wrong parameters!\n");
        }
        

        private bool RequiredStringValueIsEmpty( string value )
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        private void ThrowErrorValidation( string error )
        {
            throw new ArgumentException(error);
        }
    }
}
