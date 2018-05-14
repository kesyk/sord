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
namespace CorePract.Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    public class IssuesController : Controller
    {
        private static RegIssueParamsValidator _paramsValidator;
        private IConfiguration _config;
        private readonly IssuesStorage _issuesStorage;

        public IssuesController (IssuesStorage issuesStorage, IConfiguration Configuration)
        {
            _issuesStorage = issuesStorage;
            _config = Configuration;
        }

        [HttpPost("reg")]
        public ActionResult Registration( [FromBody]  IssueDto issue )
        {
            try
            {
                if (_paramsValidator == null) { _paramsValidator = new RegIssueParamsValidator(); }
                if(_paramsValidator.ValidateQueryParams(issue)==false)
                {
                    BadRequest("Некорректные параметры");
                }

                _issuesStorage.UpdateRawIssue(ref issue);
                _issuesStorage.Insert(issue);

                RabbitMqConnector rabbitCon = new RabbitMqConnector();
                rabbitCon.Connect(_config["RabbitMq:user"], _config["RabbitMq:vHost"], _config["RabbitMq:password"], _config["RabbitMq:host"]);

                IModel channel = rabbitCon.conn.CreateModel();

                rabbitCon.Send(
                    channel,
                    _config["RabbitMq:exchange"],
                    _config["RabbitMq:queueNew"], 
                    _config["RabbitMq:routingKeyNew"], 
                    JsonConvert.SerializeObject(issue));

                rabbitCon.Disconnect();

                return Ok(issue.IssueId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet]
        public ActionResult ShowAllIssues()
        {
            try
            {
                if(_issuesStorage.isEmpty() == false)
                {
                    return Ok(_issuesStorage.GetIssues());
                }

                return NotFound("Заявок нет");
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }

        }

        [HttpGet("{id}")]
        public ActionResult ShowIssueById(string id)
        {
            try
            {
                if(_issuesStorage.isEmpty() == false)
                { 
                    if(_issuesStorage.GetIssueById(id)!=null)
                    {
                       return NotFound("Заявка не найдена!");
                    }
                    return Ok(_issuesStorage.GetIssueById(id));
                }

                return NotFound("Заявок нет");
                
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }

        }


    }
}
