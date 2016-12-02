using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Notification.Adapters;
using Notification.Model;
using Notification.Service;

namespace Notification
{
    [Activity(Label = "ReservationActivity")]
    public class ReservationActivity : Activity
    {
        private Context _mContext;
        private AppPreferences _ap;
        private string _userId;
        private ListView _reservationListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Reservation);
            // Create your application here

            _mContext = Android.App.Application.Context;
            _ap = new AppPreferences(_mContext);
            _userId = _ap.getUserIdKey();

            ReservationService reservationService = new ReservationService();

            _reservationListView = FindViewById<ListView>(Resource.Id.reservationLv);
            _reservationListView.ChoiceMode = ChoiceMode.Single;
            _reservationListView.Adapter = new ReservationAdapter(this, reservationService.GetReservations());

            RegisterForContextMenu(_reservationListView);
            _reservationListView.ItemClick += _reservationListView_ItemClick;
        }

        private void _reservationListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            MenuInflater.Inflate(Resource.Menu.ReservationMenu, menu);

            base.OnCreateContextMenu(menu, v, menuInfo);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            var model = Cast(_reservationListView.GetItemAtPosition(info.Position));

            switch (item.ItemId)
            {
                //case Resource.Id.reservationViewMap:
                //    Toast.MakeText(this.ApplicationContext, "Respond", ToastLength.Short).Show();
                //    RespondDisaster(model.Id);
                //    return true;
                case Resource.Id.reservationViewMap:
                    var activity2 = new Intent(this, typeof(MapActivity));
                    activity2.PutExtra("lat", model.Lat==0?"": model.Lat.ToString(CultureInfo.CurrentCulture));
                    activity2.PutExtra("lng", model.Lat==0? "" : model.Lng.ToString(CultureInfo.CurrentCulture));
                    StartActivity(activity2);
                    return true;
                case Resource.Id.reservationCancel:
                    Toast.MakeText(this.ApplicationContext, "Cancel", ToastLength.Short).Show();
                    return true;
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        public static ReservationModel Cast(Object obj)
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as ReservationModel;
        }
    }
}