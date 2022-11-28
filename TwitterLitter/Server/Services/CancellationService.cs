using TwitterLitter.Server.Interfaces;

namespace TwitterLitter.Server.Classes
{
    public class CancellationService : ICancellationService
    {
        public static CancellationTokenSource? CancellationTokenSource;
        public CancellationService()
        {
            if (CancellationTokenSource == null)
            {
                CancellationTokenSource = new CancellationTokenSource();
            }
        }

        public void Cancel()
        {
            CancellationService.CancellationTokenSource.Cancel();
        }
    }
}
