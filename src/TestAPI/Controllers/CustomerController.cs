using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet("get/{id}")]
        public ContentResult GetCustomerById(string id)
        {
            using (var db = new LiteDatabase(@"TestDatabase.db"))
            {
                var table = db.GetCollection<Customer>("customers");

                var customer = table.Find(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();

                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = customer != null ? JsonConvert.SerializeObject(customer) : string.Empty,
                    ContentType = "application/json"
                };
            }
        }

        [HttpPost("create")]
        public ContentResult CreateCustomer([FromBody] Customer customer)
        {
            using (var db = new LiteDatabase(@"TestDatabase.db"))
            {
                var table = db.GetCollection<Customer>("customers");

                var c = table.Find(x => x.Id == customer.Id);

                if (c.Any())
                {
                    return new ContentResult
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Content = $"Customer {customer.Id} already exists",
                        ContentType = "application/json"
                    };
                }

                table.Insert(customer);

                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = $"Customer {customer.Id} created successfully",
                    ContentType = "application/json"
                };
            }
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}