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
    public class LocationModel
    {
        public int UserId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}