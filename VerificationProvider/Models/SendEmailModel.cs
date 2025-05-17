namespace VerificationProvider.Models;

public class SendEmailModel
{
    public List<string> Recipients { get; set; } = [];

    public string Subject { get; set; } = null!;

    public string PlainText { get; set; } = null!;

    public string Html { get; set; } = null!;
}