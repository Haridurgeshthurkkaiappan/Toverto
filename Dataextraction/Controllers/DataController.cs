using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DataextractionTask;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;


namespace Dataextraction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public DataModel _obj;
        public DataController(DataModel obj)
        {
            _obj = obj;
        }
        public ActionResult GetMembers()
        {
            IEnumerable<DataModel> members = null;



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
  
                var responseTask = client.GetAsync("posts");
                responseTask.Wait();
                var result = responseTask.Result;

           
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<DataModel>>();
                    readTask.Wait();

                    members = readTask.Result;
                }
                else
                {
                    
                    members = Enumerable.Empty<DataModel>();
                    var error = result.Content.ReadAsStringAsync().Result;
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return (ActionResult)members;

        }

    }
}