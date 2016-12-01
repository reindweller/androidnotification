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
using Notification.Model;

namespace Notification.Service
{
    public class ReservationService
    {
        public List<ReservationModel> GetReservations(int userId)
        {
            return new List<ReservationModel>
            {
                new ReservationModel
                {
                    ReservationId = 1,
                    ReservationDate = DateTime.Now
                },
                new ReservationModel
                {
                    ReservationId = 1,
                    ReservationDate = DateTime.Now.AddDays(-2)
                }
            };
        }
    }
}