using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using CorePract.Controllers.Validators;
using CoreTest.DTO;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
namespace CorePract.Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    public class IssuesController : Controller
    {
        private static List<IssueDto> _issues;
        private static RegIssueParamsValidator _paramsValidator;

        [HttpPost("reg")]
        public string Registration( [FromBody]  IssueDto issue )
        {
            try
            {
                if (_paramsValidator == null) { _paramsValidator = new RegIssueParamsValidator(); }
                _paramsValidator.ValidateQueryParams(issue);

                if (_issues == null) { _issues = new List<IssueDto>(); }
                _issues.Add(issue);

                return issue.IdIssue;
            }
            catch (Exception ex)
            {
                if(ex.GetType()==typeof(ArgumentException))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

                return ex.Message;
            }

        }

        [HttpGet]
        public string ShowAllIssues()
        {
            try
            {
                string output = "";

                if(_issues == null)
                {
                    output = "Заявок нет";
                }
                else
                {
                    _issues
                        .ForEach((issue) => 
                        {
                            output = output + JsonConvert.SerializeObject(issue) + "\n";
                        });
                }

                return output;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return ex.Message;
            }

        }

        [HttpGet("{id}")]
        public string ShowIssueById(string id)
        {
            try
            {
                string output = "";

                if(_issues == null)
                {
                    output = "Не удалось найти заявку";
                }
                else
                {
                    _issues
                        .Where(issue => issue.IdIssue.Equals(id))
                        .ToList()
                        .ForEach(issue =>
                        {
                            output = output + JsonConvert.SerializeObject(issue) + "\n";
                        });
                }

                return output;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return ex.Message;
            }

        }


    }
}
