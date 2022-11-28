namespace TwitterLitter.Server.Interfaces
{
    public interface ICancellationService
    {
        static CancellationTokenSource? CancellationTokenSource { get; set; }

        public void Cancel();
    }
}
