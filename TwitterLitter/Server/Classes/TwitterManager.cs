using TwitterLitter.Server.Interfaces;
using TwitterLitter.Shared;

namespace TwitterLitter.Server.Classes
{
    public class TwitterManager:IHostedService
    {
        ITwitterSampleStreamClient sampleStreamClient;
        public TwitterManager(ITwitterSampleStreamClient _sampleStreamClient)
        {
            sampleStreamClient = _sampleStreamClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => sampleStreamClient.ProcessStream(cancellationToken)).ContinueWith(LogResult);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await sampleStreamClient.Dispose();
        }

        private static void LogResult(Task<string> task)
        {
            string result = task.Result;
            // we would likely want to do something with this.  Update a status to be read by the UI, perhaps?
        }
    }
}
