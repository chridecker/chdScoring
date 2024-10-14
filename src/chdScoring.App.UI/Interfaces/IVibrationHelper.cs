namespace chdScoring.App.UI.Interfaces
{
    public interface IVibrationHelper
    {
        void Vibrate(TimeSpan duration);
        Task Vibrate(int repeat, TimeSpan duration, CancellationToken cancellationToken = default);
        Task Vibrate(int repeat, TimeSpan duration, TimeSpan breakDuration, CancellationToken cancellationToken = default);
    }
}
