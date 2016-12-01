using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Notification.Model;

namespace Notification
{
    [Activity(Label = "Notification", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Context _mContext;
        private AppPreferences _ap;
        private string _userId;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _mContext = Android.App.Application.Context;
            _ap = new AppPreferences(_mContext);
            _userId = _ap.getUserIdKey();

            if (string.IsNullOrEmpty(_userId))
            {
                Console.WriteLine("please login");
                StartActivity(typeof(LoginActivity));
            }


            

            Button launchBtn = FindViewById<Button>(Resource.Id.launchButton);
            launchBtn.Click += delegate
            {
                LaunchNotification("sample notif", "something happened");
            };

            Button mapBtn = FindViewById<Button>(Resource.Id.mapBtn);
            mapBtn.Click += MapBtn_Click;

            Button logoutBtn = FindViewById<Button>(Resource.Id.logoutBtn);
            logoutBtn.Click += LogoutBtn_Click;

            //Timer
        }

        private void MapBtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MapActivity));
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            _ap.saveUserIdKey(string.Empty);
            StartActivity(typeof(LoginActivity));
        }

        //public void TimerMethod()
        //{
        //    TimerExampleState s = new TimerExampleState();

        //    // Create the delegate that invokes methods for the timer.
        //    TimerCallback timerDelegate = new TimerCallback(LaunchNotification);

        //    // Create a timer that waits one second, then invokes every second.
        //    Timer timer = new Timer(timerDelegate, s, 1000, 1000);

        //    // Keep a handle to the timer, so it can be disposed.
        //    s.tmr = timer;

        //    // The main thread does nothing until the timer is disposed.
        //    while (s.tmr != null)
        //        Thread.Sleep(0);
        //    Console.WriteLine("Timer example done.");
        //}

        private void LaunchNotification(string title, string message)
        {
            // Get the notifications manager:
            NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            // Instantiate the notification builder:
            Android.App.Notification.Builder builder = new Android.App.Notification.Builder(this)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.ic_notification)
                .SetAutoCancel(true);


            // Build the notification:
            Android.App.Notification notification = builder.Build();

            notification.Defaults |= NotificationDefaults.Sound;

            // Notification ID used for all notifications in this app.
            // Reusing the notification ID prevents the creation of 
            // numerous different notifications as the user experiments
            // with different notification settings -- each launch reuses
            // and updates the same notification.
            const int notificationId = 1;

            // Launch notification:
            notificationManager.Notify(notificationId, notification);
        }
    }
}

