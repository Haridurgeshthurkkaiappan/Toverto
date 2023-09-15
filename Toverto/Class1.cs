using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace DataextractionTask
{
   

    public class Dataextraction
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiUrl = "https://jsonplaceholder.typicode.com/posts";
        private static readonly string exportType = Configuration["ExportType"]; 


        public static async Task Main(string[] args)
        {
            try
            {
                string data = await DownloadDataAsync();
                List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(data);

                foreach (var post in posts)
                {
                    post.HashID = GenerateHashID();

                switch (exportType)
                {
                    case "JSON":
                        ExportToJson(posts);
                        break;
                    case "CSV":
                        ExportToCsv(posts);
                        break;
                    case "SQL":
                        InsertIntoSql(posts);
                        break;
                    default:
                        Console.WriteLine("Invalid ExportType specified in configuration.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static async Task<string> DownloadDataAsync()
        {
            int maxRetries = 3;
            int currentRetry = 0;

            while (currentRetry < maxRetries)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (Exception)
                {
                    currentRetry++;
                }
            }

            throw new Exception("Failed to retrieve data from API after multiple retries.");
        }

        
    }
}
