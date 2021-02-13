using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AZFunctionApp
{
    public static class TodoApi
    {
        static List<ToDoItem> Items = new List<ToDoItem>();

        [FunctionName("GetAllItems")]
        public static async Task<IActionResult> GetAllItems(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult(Items.Where(x => x.IsCompleted == false));
        }

        [FunctionName("CreateItem")]
        public static async Task<IActionResult> CreateItem(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "todo")] HttpRequest req,
    ILogger log)
        {
            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ToDoItemInputModel>(reqBody);

            ToDoItem newItem = new ToDoItem(input.Description);
            newItem.Id = Guid.NewGuid().ToString();
            newItem.IsCompleted = false;

            Items.Add(newItem);

            return new OkObjectResult(newItem);
        }

        [FunctionName("UpdateItem")]
        public static async Task<IActionResult> UpdateItem(
[HttpTrigger(AuthorizationLevel.Function, "put", Route = "todo/{id}")] HttpRequest req,
ILogger log, string id)
        {

            var currentItem = Items.Where(x => x.Id == id).FirstOrDefault();
            if(currentItem == null)
            {
                return new NotFoundObjectResult($"Item Not Found With Id {id}");
            }

            currentItem.IsCompleted = true;
            return new OkObjectResult(currentItem);
        }

        [FunctionName("GetItemByid")]
        public static async Task<IActionResult> GetItem(
[HttpTrigger(AuthorizationLevel.Function, "Get", Route = "todo/{id}")] HttpRequest req,
ILogger log, string id)
        {
            var currentItem = Items.Where(x => x.Id == id).FirstOrDefault();
            if (currentItem == null)
            {
                return new NotFoundObjectResult($"Item Not Found With Id {id}");
            }
            return new OkObjectResult(currentItem);
        }
    }
}
