using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Notification.Model
{
    public class NotificationMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}