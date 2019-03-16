using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Integreat.Localization;
using Integreat.Shared.Data.Sender.Targets;
using Plugin.Connectivity;

namespace Integreat.Shared.Data.Sender
{
    public class DataSenderProvider
    {

        public readonly FeedbackDataSender FeedbackDataSender;

        public DataSenderProvider(FeedbackDataSender feedbackDataSender)
        {
            FeedbackDataSender = feedbackDataSender;
        }

        public static async Task ExecuteSendMethod(IDataSender caller,
                    Func<Task> senderMethod, Action<string> errorLogAction)
        {
            //check if there is a internet connection
            if (!CrossConnectivity.Current.IsConnected) {
                errorLogAction?.Invoke(AppResources.ErrorInternet);
                Debug.WriteLine("Can't send data. No internet connection");
                return;
            }

            // task that will send the data
            var task = Task.Run(() =>
            {
                try
                {
                    senderMethod();
                }
                catch (Exception e)
                {
                    errorLogAction?.Invoke(AppResources.ErrorGeneral);
                    Debug.WriteLine("Error when loading data: " + e);
                }
            });

            // start the work task and a task which will complete after a timeout simultaneously.
            const int timeout = 10000; // 10 seconds timeout
            if (await Task.WhenAny(task, Task.Delay(timeout)) != task)
            {
                // timeout logic
                Debug.WriteLine("Timeout sending data");
                errorLogAction?.Invoke(AppResources.ErrorGeneral);
            }
        }
    }
}
