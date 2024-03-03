using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using WebApplication1.Services;
using WebApplication1.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDataService _dataService;
        private readonly HttpClient _httpClient;

        public class Data
        {
            public string RequestData { get; set; }
        }

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _dataService = new DataService();
            _configuration = configuration;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        [HttpGet("data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> data([FromQuery] List<string> symbols)
        {
            StockData dataa = GetDataFromApi(symbols);
            string sentiment = await GetSentimentFromApi(new { inputs =  "market is going through tough times"});
            string news = await GetNewsFromApi();
            var temp = new Entry()
            {
                Headline = "placeholder",
                Stock = dataa.MetaData.Symbol,
                Value = Double.Parse(dataa.TimeSeries.First().Value.Close, CultureInfo.InvariantCulture),
                ValueAfterHour = 0
            };


            _dataService.SaveData(temp);
            return Ok(dataa);

        }

        private StockData GetDataFromApi(List<string> symbols)
        {
            string API_KEY = "placeholder";
            string QUERY_URL = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=" + symbols.First() + "&interval=5min&apikey=" + API_KEY;
            Uri queryUri = new(QUERY_URL);

            string response = _httpClient.GetStringAsync(queryUri).Result;
            StockData json_data = JsonConvert.DeserializeObject<StockData>(response);

            if (json_data != null)
            {
                return json_data;
            }
            else return null;
            
        }

        private async Task<string> GetSentimentFromApi(object data)
        {
            string QUERY_URL = "https://api-inference.huggingface.co/models/ProsusAI/finbert";
            string token = "placeholder";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(QUERY_URL, content);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            return "";
        }

        private async Task<string> GetNewsFromApi()
        {
            string API_KEY = "placeholder";
            string QUERY_URL = "https://api.scrape-it.cloud/scrape/google/serp?q=Bayer&location=Austin%2CTexas%2CUnited+States&tbm=nws&deviceType=desktop";
            _httpClient.DefaultRequestHeaders.Add("x-api-key", API_KEY);
            string response = await _httpClient.GetStringAsync(QUERY_URL);
            var json_data = JsonConvert.DeserializeObject<dynamic>(response);
            Console.WriteLine(json_data);
            return null;
        }
    }
}
