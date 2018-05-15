using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using CorePract.Controllers.Validators;
using CorePract.Dto;

using CorePract.Messaging.RabbitMQ;
using CorePract.Services;
using RabbitMQ.Client;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
namespace CorePract.Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    public class IssuesController : Controller
    {
        private  RegIssueParamsValidator _paramsValidator;
        private IConfiguration _config;
        private readonly IssuesStorage _issuesStorage;

        public IssuesController (IssuesStorage issuesStorage, IConfiguration Configuration, RegIssueParamsValidator paramsValidator )
        {
            _issuesStorage = issuesStorage;
            _config = Configuration;
            _paramsValidator = paramsValidator;
        }

        [HttpPost("reg")]
        public string Registration( [FromBody]  IssueRawDto issueRaw )
        {
            try
            {
                if (_paramsValidator == null) { _paramsValidator = new RegIssueParamsValidator(); }
                if(!_paramsValidator.ValidateQueryParams(issueRaw))
                {
                    return "Некорректные параметры";
                }

                IssueDto issue = _issuesStorage.CreateIssue(issueRaw);
                _issuesStorage.Insert(issue);

                RabbitMqConnector rabbitCon = new RabbitMqConnector();
                rabbitCon.Connect(_config["RabbitMq:user"], _config["RabbitMq:vHost"], _config["RabbitMq:password"], _config["RabbitMq:host"]);

                IModel channel = rabbitCon.conn.CreateModel();

                rabbitCon.Send(
                    channel,
                    _config["RabbitMq:exchange"],
                    _config["RabbitMq:routingKeyNew"], 
                    JsonConvert.SerializeObject(issue));

                rabbitCon.Disconnect();

                return issue.IssueId;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }

        }

        [HttpGet]
        public IEnumerable<IssueDto> ShowAllIssues()
        {
            try
            {
                if(!_issuesStorage.isEmpty())
                {
                    Response.StatusCode = (int)HttpStatusCode.Found;
                    return _issuesStorage.GetIssues();
                }
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return null;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }

        }

        [HttpGet("{id}")]
        public IssueDto ShowIssueById(string id)
        {
            try
            {
                if(!_issuesStorage.isEmpty())
                { 
                    if(!_issuesStorage.hasIssue(id))
                    {
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return null;
                    }
                    Response.StatusCode = (int)HttpStatusCode.Found;
                    return _issuesStorage.GetIssueById(id);
                }
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return null;
                
            }
            catch (Exception ex)
            {
    
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;

            }

        }


    }
}
