using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Notification.Model;

namespace Notification.Service
{
    public class ReservationService
    {
        public List<ReservationModel> GetReservations()
        {
            var response = ApiCallService.GetAllRequest(Constants.ApiUrl + "transaction/getreserved");

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                //Stream responseStream = response.GetResponseStream();
                string responseStr = reader.ReadToEnd();
                var modelList = JsonConvert.DeserializeObject<IEnumerable<ReservationModel>>(responseStr);
                return modelList.ToList();
            }

        }

        public async void UpdateLocation(LatLng loc, int userId)
        {
            var loct = new LocationModel
            {
                UserId = userId,
                Lat = loc.Latitude,
                Lng = loc.Longitude
            };

            var result = await ApiCallService.UpdateRequestAsync(loct, Constants.ApiUrl + "reservation/updatelocation");
            var r = result;
        }
    }
}