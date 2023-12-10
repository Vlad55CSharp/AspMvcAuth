using System.Net.Mail;
namespace AspMvcAuth.Models
{
	public static class EmailHelperExtensions
	{
		public static void AddEmailHelper(this IServiceCollection services, EmailHelperOptions options)
		{
			services.AddTransient((service)=>new EmailHelper(options));
		}
	}

	public class EmailHelper
	{
        public EmailHelperOptions Options { get; }

		public EmailHelper(EmailHelperOptions options)
		{
			Options = options;
		}

		private bool Send(string userEmail, string message, string subject = "")
		{
            MailMessage mailMessage = new()
            {
                From = new MailAddress(Options.From)
            };
            mailMessage.To.Add(new MailAddress(userEmail));

            if (string.IsNullOrEmpty(subject))
               mailMessage.Subject = Options.Subject;
            else
                mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message;

            SmtpClient client = new()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(Options.Login, Options.Password),
                Host = Options.Host,
                Port = Options.Port,
                EnableSsl = Options.EnableSSL
            };

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch 
            {
                return false;
            }
        }

		public bool SendEmail(string userEmail, string confirmationLink)
		{
            return Send(userEmail, confirmationLink);
		}

        public bool SendEmailTwoFactorCode(string userEmail, string code)
        {
            return Send(userEmail, code, "Код подтверждения");
        }
    }
}
