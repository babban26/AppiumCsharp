using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System;

public class JiraClient
{
    private const string JiraBaseUrl = "https://uxin.atlassian.net/";
    private const string Username = "Babban.Chandak@cerebrent.com";
    private const string Password = "ATATT3xFfGF0XeHdcVS2aZHaLpfFpd5tHfjDIzPnMHQYtXN-3KLLFbA_rtWQTBqLNliDpU_Z1ceO28XPHGK9NkTzn_RP0mDT9A7asMBgs85HlbbC3XUl8jxMw7NyKaQkfVAYgA6O9sTttW3nDQHyJNnTmoGhfxHyXtNOWjAn7ZRIWIiiDakD7jo=73248302";
    private const string ProjectKey = "FRAIS";

    private readonly RestClient _client;

    public JiraClient()
    {
        _client = new RestClient(JiraBaseUrl)
        {
            Authenticator = new HttpBasicAuthenticator(Username, Password)
        };
    }

    public void CreateBug(string summary, string customfield_10083, string description)
    {
       // var request2 = new RestRequest("rest/api/2/field", Method.GET);
        var request = new RestRequest("rest/api/2/issue", Method.POST)
            .AddJsonBody(new
            {
                fields = new
                {
                    project = new { key = ProjectKey },
                    summary,
                    description,
                    customfield_10083,
                    issuetype = new { name = "Bug" }
                }
            });

        var response = _client.Execute(request);

        if (!response.IsSuccessful)
        {
            Console.WriteLine($"Failed to create Jira bug. Error: {response.ErrorMessage}");
        }
        else
        {
            Console.WriteLine("Jira bug created successfully."+response.Content);
        }
    }
}


