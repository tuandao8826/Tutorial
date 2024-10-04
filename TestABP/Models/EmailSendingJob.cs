using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace TestABP.Models
{
    public class EmailSendingJob : AsyncBackgroundJob<EmailSendingArgs>, ITransientDependency
    {
        //private readonly IEmailSender _emailSender;

        //public EmailSendingJob(IEmailSender emailSender)
        //{
        //    _emailSender = emailSender;
        //}

        //public override async Task ExecuteAsync(EmailSendingArgs args)
        //{
        //    await _emailSender.SendAsync(
        //        args.EmailAddress,
        //        args.Subject,
        //        args.Body
        //    );
        //}
    }
}
