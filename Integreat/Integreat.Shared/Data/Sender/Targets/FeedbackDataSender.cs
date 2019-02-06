using System;
using System.Threading.Tasks;
using Integreat.Shared.Models;

namespace Integreat.Shared.Data.Sender.Targets
{
    public class FeedbackDataSender : IDataSender
    {
        private readonly IDataLoadService _dataLoadService;
        public string FileName => throw new NotImplementedException();

        public FeedbackDataSender(IDataLoadService dataLoadService) {
            _dataLoadService = dataLoadService;
        }

        public Task Send(Language language, Location location, Feedback feedback, Action<string> errorLogAction = null) {
            return DataSenderProvider.ExecuteSendMethod(this, () => _dataLoadService.SendFeedback(feedback, language, location), errorLogAction);
        }
    }
}
