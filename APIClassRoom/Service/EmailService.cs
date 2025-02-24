using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _siteUrl = "https://localhost:7140/complete-form"; // Update to your actual domain


    
    public async Task SendConfirmationEmailAsync(string toEmail)
    {
        string subject = "Confirm Your Email Address";
        string confirmationLink = $"{_siteUrl}?email={WebUtility.UrlEncode(toEmail)}";

        string body = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                <h2 style='color: #2c3e50; text-align: center;'>Welcome to Our Platform!</h2>
                <p style='color: #555; text-align: center;'>Please confirm your email address to complete your registration.</p>
                
                <div style='text-align: center; margin: 20px 0;'>
                    <a href='{confirmationLink}' style='display: inline-block; padding: 12px 25px; font-size: 16px; color: #fff; background-color: #3498db; text-decoration: none; border-radius: 5px;'>
                        Confirm Email
                    </a>
                </div>

                <p style='color: #777; font-size: 14px; text-align: center;'>If you didn't request this, you can safely ignore this email.</p>
                
                <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;'>
                
                <p style='color: #888; font-size: 12px; text-align: center;'>© 2024 Your Company. All rights reserved.</p>
            </div>
        ";

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("khadidja_zergane@univ-biskra.dz", "wqpj aelm cxvt mniw"),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("kadidja_zergane@univ-biskra.dz", "Booking.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}


