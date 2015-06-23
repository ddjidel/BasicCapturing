using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;

namespace UniversalJupiter.Helpers
{
    public static class xRMConnector
    {
        public class Lead
        {
            public string Subject { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NoteSubject { get; set; }
            public string FileName { get; set; }
            public string MimeType { get; set; }
            public byte[] Data { get; set; }
        }
        public async static void CreateLead(string subject, string firstName, string lastName, string noteSubject, string fileName, string mimeType, byte[] data)
        {
            JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            Lead lead = new Lead();
            lead.Subject = subject;
            lead.FirstName = firstName;
            lead.LastName = lastName;
            lead.NoteSubject = noteSubject;
            lead.FileName = fileName;
            lead.MimeType = mimeType;
            lead.Data = data;

            var json = JsonConvert.SerializeObject(lead);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            try
            {
                var uri = "http://xrmconnector.azurewebsites.net/api/Lead";
                HttpClient request = new HttpClient();
                request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await request.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (HttpRequestException hre)
            {
                
            }
        }
    }
}
