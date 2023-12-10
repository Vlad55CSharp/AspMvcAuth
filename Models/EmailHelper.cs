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

		public bool SendEmail(string userEmail, string confirmationLink)
		{
			MailMessage mailMessage = new()
            {
				From = new MailAddress(Options.From)
			};
			mailMessage.To.Add(new MailAddress(userEmail));

			mailMessage.Subject = Options.Subject;
			mailMessage.IsBodyHtml = true;
			mailMessage.Body = confirmationLink;

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
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return false;
		}
	}
}
