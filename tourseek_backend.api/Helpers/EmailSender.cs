using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace tourseek_backend.api.Helpers
{
    public static class EmailSender
    {
        public static async Task SendEmail()
        {
            MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("17fbec5b8ff0c5babdc18efba690e7d6"), Environment.GetEnvironmentVariable("f380b1dce9e246325e498bcc316894d5"))
            {
                BaseAdress = "",
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "mohamed.eldefrawy13026011@aiet.edu.eg"},
        {"Name", "Mohamed"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          "mohamed.eldefrawy13026011@aiet.edu.eg"
         }, {
          "Name",
          "Mohamed"
         }
        }
       }
      }, {
       "Subject",
       "Greetings from Mailjet."
      }, {
       "TextPart",
       "My first Mailjet email"
      }, {
       "HTMLPart",
       "<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
      }, {
       "CustomID",
       "AppGettingStartedTest"
      }
     }
             });
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(response.GetData());
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }
        }
    }
}
