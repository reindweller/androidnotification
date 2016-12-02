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
    public class ReservationModel
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public string ReservationCode { get; set; }
        public int CarId { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public double AmountDue { get; set; }
        public string CustomerContact { get; set; }
        public string CarName { get; set; }
        public string CarModel { get; set; }
        public string Id1 { get; set; }
        public string Id2 { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}