using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace BuhleProject.Models
{
    public class Email
    {
        public static void send()
        {
            Execute().Wait();
        }
        public static async Task send(string UserName, string Email, string applyDate,string amount,string Subject,string Content )
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@24houremergencystudentloan.co.za", "24 hour student loan");
            var subject = Subject;
            var to = new EmailAddress(@Email, UserName);
            var plainTextContent = Content;

            var htmlContent = @"<p> "+Content+" </p><br/><strong>email:emergencystudentloan@gmail.com || contact us on whatsapp: 065 882 7980 </strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }


        static async Task Execute()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "24 hour student loan");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("melusimgwenya@gmail.com", "melusi mgwenya");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}


