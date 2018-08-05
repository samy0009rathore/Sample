using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var baseURI = new Uri("https://navqna.azurewebsites.net/qnamaker");
            var knowledgeBaseID = "/knowledgebases/5334f73a-897e-4fb6-9ce3-44fafacc32cc/generateAnswer";
            // EndpointKey 1389ba80-08fc-4cb2-a19b-209eda40d675
            var finalURL = new Uri(baseURI, knowledgeBaseID);

            using (var client = new HttpClient())
            {
                // Set the client properties
                client.Timeout = TimeSpan.FromSeconds(20);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("EndpointKey", "1389ba80-08fc-4cb2-a19b-209eda40d675");
                var obj = new { question = "what is your name" };
                HttpContent body = new StringContent(JsonConvert.SerializeObject(obj));
                body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                string content;
                var result = client.PostAsync(finalURL, body).Result;
                if (!result.IsSuccessStatusCode)
                {
                    content = result.Headers.FirstOrDefault(x => x.Key == "ERROR").Value.FirstOrDefault();

                    if (content.Length > 0 && content.ToUpper().Contains("ERROR"))
                        content = content.Replace("%0d", "").Replace("%0a", "\r");
                    throw new Exception(content);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QnAMakerService hrQnAService = new QnAMakerService("https://navqna.azurewebsites.net/qnamaker", "5334f73a-897e-4fb6-9ce3-44fafacc32cc", "1389ba80-08fc-4cb2-a19b-209eda40d675");
            var result = hrQnAService.GetAnswer("what is your name");
        }
    }
}
