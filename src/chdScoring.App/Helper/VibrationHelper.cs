namespace chdScoring.App.Helper
{
    public class VibrationHelper : IVibrationHelper
    {
        public void Vibrate(TimeSpan duration)
        {
            try
            {
                Vibration.Default.Vibrate(duration);
            }
            catch { }
        }

        public Task Vibrate(int repeat, TimeSpan duration, CancellationToken cancellationToken) => this.Vibrate(repeat, duration, duration, cancellationToken);

        public async Task Vibrate(int repeat, TimeSpan duration, TimeSpan breakDuration, CancellationToken cancellationToken)
        {
            for (int i = 0; i < repeat; i++)
            {
                this.Vibrate(duration);
                await Task.Delay(duration, cancellationToken);
            }
        }

    }
    public interface IVibrationHelper
    {
        void Vibrate(TimeSpan duration);
        Task Vibrate(int repeat, TimeSpan duration, CancellationToken cancellationToken = default);
        Task Vibrate(int repeat, TimeSpan duration, TimeSpan breakDuration, CancellationToken cancellationToken = default);
    }
}
