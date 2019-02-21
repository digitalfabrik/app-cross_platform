using System;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Models.Feedback;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Data.Sender.Targets
{
    public class FeedbackDataSender : IDataSender
    {
        private readonly IDataLoadService _dataLoadService;
        public string FileName => throw new NotImplementedException();

        public FeedbackDataSender(IDataLoadService dataLoadService) {
            _dataLoadService = dataLoadService;
        }

        public Task Send(Language language, Location location, IFeedback feedback, FeedbackType feedbackType, Action<string> errorLogAction = null)
            => DataSenderProvider.ExecuteSendMethod(this, ()
                 => _dataLoadService.SendFeedback(feedback, language, location, feedbackType.GetStringValue()), errorLogAction);
    }
}
