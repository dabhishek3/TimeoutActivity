using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TimeOutApplication
{
    //public sealed class SessionManager
    //{
    //    static readonly Lazy<SessionManager> lazy =
    //        new Lazy<SessionManager>(() => new SessionManager());

    //    public static SessionManager Instance { get { return lazy.Value; } }

    //    SessionManager()
    //    {
    //        this.SessionDuration = TimeSpan.FromMinutes(3);
    //        this.sessionExpirationTime = DateTime.FromFileTimeUtc(0);
    //    }

    //    /// <summary>
    //    /// The duration of the session, by default this is set to 5 minutes.
    //    /// </summary>
    //    public TimeSpan SessionDuration;

    //    /// <summary>
    //    /// The OnSessionExpired event is fired when the session timer expires.
    //    /// This event is not fired if the timer is stopped manually using 
    //    /// EndTrackSession.
    //    /// </summary>
    //    public EventHandler OnSessionExpired;

    //    /// <summary>
    //    /// The session expiration time.
    //    /// </summary>
    //    DateTime sessionExpirationTime;

    //    /// <summary>
    //    /// A boolean value indicating wheter a session is currently active.
    //    /// Is set to true when StartTrackSessionAsync is called. Becomes false if 
    //    /// the session is expired manually or by expiration of the session 
    //    /// timer.
    //    /// </summary>
    //    public bool IsSessionActive { private set; get; }

    //    /// <summary>
    //    /// Starts the session timer.
    //    /// </summary>
    //    /// <returns>The track session async.</returns>
    //    public async Task StartTrackSessionAsync()
    //    {
    //        this.IsSessionActive = true;

    //        ExtendSession();

    //        await StartSessionTimerAsync();
    //    }

    //    /// <summary>
    //    /// Stop tracking a session manually. The OnSessionExpired will not be 
    //    /// called.
    //    /// </summary>
    //    public void EndTrackSession()
    //    {
    //        this.IsSessionActive = false;

    //        this.sessionExpirationTime = DateTime.FromFileTimeUtc(0);
    //    }

    //    /// <summary>
    //    /// If the session is active, then the session time is extended based 
    //    /// on the current time and the SessionDuration.
    //    /// duration.
    //    /// </summary>
    //    public void ExtendSession()
    //    {
    //        if (this.IsSessionActive == false)
    //        {
    //            return;
    //        }

    //        this.sessionExpirationTime = DateTime.Now.Add(this.SessionDuration);
    //    }

    //    /// <summary>
    //    /// Starts the session timer. When the session is expired and still 
    //    /// active the OnSessionExpired event is fired. 
    //    /// </summary>
    //    /// <returns>The session timer async.</returns>
    //    async Task StartSessionTimerAsync()
    //    {
    //        if (this.IsSessionActive == false)
    //        {
    //            return;
    //        }

    //        while (DateTime.Now < this.sessionExpirationTime)
    //        {
    //            await Task.Delay(1000);
    //        }

    //        if (this.IsSessionActive && this.OnSessionExpired != null)
    //        {
    //            this.IsSessionActive = false;

    //            this.OnSessionExpired.Invoke(this, null);
    //        }
    //    }
    //}





    public sealed class SessionManager
    {
        private static readonly Lazy<SessionManager> lazy = new Lazy<SessionManager>();
        public static SessionManager Instance { get { return lazy.Value; } }

        public NavigationPage MainPage { get; private set; }
        public Xamarin.Forms.Page CurrentPage { get; }

        private Stopwatch StopWatch = new Stopwatch();
        private readonly int _sessionThreasholdMinutes = 1;
        public SessionManager()
        {
            SessionDuration = TimeSpan.FromMinutes(_sessionThreasholdMinutes);
        }
        private TimeSpan SessionDuration;
        public void EndSession()
        {
            if (StopWatch.IsRunning)
            {
                StopWatch.Stop();
            }
        }
        public void ExtendSession()
        {
            if (StopWatch.IsRunning)
            {
                StopWatch.Restart();
            }
        }
        public void StartSession()
        {
            if (!StopWatch.IsRunning)
            {
                StopWatch.Restart();
            }
            Console.WriteLine("Session Started at " + DateTime.Now.ToLongTimeString());
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                bool isTimerRunning = true;
                if (StopWatch.IsRunning && StopWatch.Elapsed.Minutes >= SessionDuration.Minutes) //User was inactive for N minutes
                {
                    RedirectAndInformInactivity();
                    EndSession();
                    isTimerRunning = false;
                }
                Console.WriteLine("Current Time Elapsed -" + StopWatch.Elapsed.ToString());
                return isTimerRunning;
            });
        }
        //TODO
        private async void RedirectAndInformInactivity()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.DisplayAlert("Session Expired", "Your session has expired", "Ok");
               
                //var answer = Application.Current.MainPage.DisplayAlert("Session Expired", "Your session has expired", "Ok");
                //if(answer!=null)
                //{
                //    await App.Current.MainPage.Navigation.PushModalAsync(new LoginView());
                //}
               
            });
        }
    }
}
