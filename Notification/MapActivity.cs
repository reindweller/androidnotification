using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Notification.Service;

namespace Notification
{
    [Activity(Label = "Map")]
    public class MapActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        private GoogleMap mMap;
        private Button btnNormal;
        private Button btnHybrid;
        private Button btnSatellite;
        private Button btnTerrain;
        private Button btnDistress;
        private EditText txtDescription;
        private Button btnSubmitDistress;
        private Button btnCancel;
        private LinearLayout layMapAction;
        private LinearLayout layDescription;

        private LatLng pos;
        private Context _mContext;
        private AppPreferences _ap;
        private int _userId;

        private Location _currentLocation;
        private LocationManager _locationManager;
        private string _locationProvider;
        readonly string[] PermissionsLocation =
   {
      Manifest.Permission.AccessCoarseLocation,
      Manifest.Permission.AccessFineLocation
    };

        const int RequestLocationId = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Map);
            // Create your application here



            //layMapAction = FindViewById<LinearLayout>(Resource.Id.layMapAction);
            //layDescription = FindViewById<LinearLayout>(Resource.Id.layDescription);

            _mContext = Android.App.Application.Context;
            _ap = new AppPreferences(_mContext);
            _userId = Convert.ToInt32(_ap.getUserIdKey());

            InitializeLocationManager();
            //if (_currentLocation == null)
            //{
            //    Toast.MakeText(this.ApplicationContext, "Can't determine the current address. Try again in a few minutes.", ToastLength.Short).Show();
            //}
            SetUpMap();

            
        }


        //async Task<LatLng> GetCurrentAddress()
        //{
        //    if (_currentLocation == null)
        //    {
        //        Toast.MakeText(this.ApplicationContext, "Can't determine the current address. Try again in a few minutes.", ToastLength.Short).Show();
        //        return pos;
        //    }

        //    Address address = await ReverseGeocodeCurrentLocation();
        //    var retPos = new LatLng(address.Latitude, address.Longitude);
        //    return retPos;
        //}

        private void InitializeLocationManager()
        {

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessFineLocation) ==
                Permission.Granted &&
                ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessCoarseLocation) ==
                Permission.Granted)
            {

                _locationManager = (LocationManager)GetSystemService(LocationService);
                Criteria criteriaForLocationService = new Criteria
                {
                    Accuracy = Accuracy.Fine
                };
                IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

                if (acceptableLocationProviders.Any())
                {
                    _locationProvider = acceptableLocationProviders.First();
                }
                else
                {
                    _locationProvider = string.Empty;
                }
                //Log.Debug(TAG, "Using " + _locationProvider + ".");

            }
            else
            {
                CallPermission();
            }

        }


        private async void CallPermission()
        {
            await GetLocationPermissionAsync();
        }

        async Task GetLocationPermissionAsync()
        {
            //Check to see if any permission in our group is available, if one, then all are
            const string permission = Manifest.Permission.AccessFineLocation;
            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                //await GetLocationAsync();
                return;
            }

            //need to request permission
            if (ShouldShowRequestPermissionRationale(permission))
            {
                //Explain to the user why we need to read the contacts
                //Snackbar.Make(layout, "Location access is required to show coffee shops nearby.", Snackbar.LengthIndefinite)
                //        .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
                //        .Show();
                Toast.MakeText(this.ApplicationContext, "Please grant permission", ToastLength.Short).Show();
                return;
            }
            //Finally request permissions with the list of permissions and Id
            RequestPermissions(PermissionsLocation, RequestLocationId);
        }

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            //Permission granted
                            //var snack = Snackbar.Make(layout, "Location permission is available, getting lat/long.", Snackbar.LengthShort);
                            //snack.Show();

                            //await GetLocationAsync();
                            InitializeLocationManager();
                            Toast.MakeText(this.ApplicationContext, "Location permission is available, getting lat/long.", ToastLength.Short).Show();
                        }
                        else
                        {
                            //Permission Denied :(
                            //Disabling location functionality
                            //var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
                            //snack.Show();
                            Toast.MakeText(this.ApplicationContext, "Location permission is denied.", ToastLength.Short).Show();
                        }
                    }
                    break;
            }
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            layMapAction.Visibility = ViewStates.Visible;
            layDescription.Visibility = ViewStates.Gone;
        }

        private void BtnSubmitDistress_Click(object sender, EventArgs e)
        {
            

            //if (string.IsNullOrEmpty(key))
            //{
            //    Toast.MakeText(this.ApplicationContext, "Please login", ToastLength.Short).Show();
            //    StartActivity(typeof(LoginActivity));
            //}

            //var model = new DisasterLocationModel
            //{
            //    UserId = new Guid(key),
            //    Lat = Convert.ToDecimal(pos.Latitude),
            //    Lng = Convert.ToDecimal(pos.Longitude),
            //    Description = txtDescription.Text,
            //    Status = DisasterLocationStatusEnum.Unresponded,
            //    DatePosted = DateTime.Now
            //};

            //var result = ApiCallService.CreateRequest(model, Constants.ApiUrl + "disasterlocation/adddisaster");
            //using (var reader = new StreamReader(result.GetResponseStream()))
            //{
            //    string responseStr = reader.ReadToEnd();
            //    //var user = JsonConvert.DeserializeObject<UserModel>(responseStr);

            //    Toast.MakeText(this.ApplicationContext, "Distress sent", ToastLength.Short).Show();
            //    StartActivity(typeof(DistressActivity));
            //}
        }

        private void BtnDistress_Click(object sender, EventArgs e)
        {
            layMapAction.Visibility = ViewStates.Gone;
            layDescription.Visibility = ViewStates.Visible;
        }

        private void SetUpMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
                //mapView.GetMapAsync(this);
            }
        }

        public async void OnMapReady(GoogleMap googleMap)
        {
            //LatLng currentPos = await GetCurrentAddress();
            //pos = currentPos;
            mMap = googleMap;


            if (_locationManager == null) return;

            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            _currentLocation = _locationManager.GetLastKnownLocation(_locationProvider);

            Address address = await ReverseGeocodeCurrentLocation();
            DisplayAddress(address);
        }

        //private void MMap_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        //{
        //    pos = e.Marker.Position;

        //}

        public async void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                //_locationText.Text = "Unable to determine your location. Try again in a short while.";
                Toast.MakeText(this.ApplicationContext, "Unable to determine your location. Try again in a short while.", ToastLength.Short).Show();
            }
            else
            {
                //_locationText.Text = string.Format("{0:f6},{1:f6}", _currentLocation.Latitude, _currentLocation.Longitude);
                Address address = await ReverseGeocodeCurrentLocation();
                DisplayAddress(address);
            }
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

        protected override async void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            _currentLocation = _locationManager.GetLastKnownLocation(_locationProvider);
            Address address = await ReverseGeocodeCurrentLocation();
            DisplayAddress(address);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        void DisplayAddress(Address address)
        {
            if (mMap == null)
            {
                return;
            }
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }
                // Remove the last comma from the end of the address.
                //_addressText.Text = deviceAddress.ToString();


                LatLng latlng = new LatLng(address.Latitude, address.Longitude);
                string latStr = Intent.GetStringExtra("lat") ?? "";
                string lngStr = Intent.GetStringExtra("lng") ?? "";

                if (latStr != "" && lngStr != "")
                {
                    latlng = new LatLng(Convert.ToDouble(latStr), Convert.ToDouble(lngStr));
                }

                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 10);
                mMap.MoveCamera(camera);

                MarkerOptions options = new MarkerOptions()
                    .SetPosition(latlng)
                    .SetTitle(address.AdminArea)
                    .SetSnippet(address.Locality)
                    .Draggable(true);

                mMap.AddMarker(options);
                pos = latlng;
                ReservationService reservationService = new ReservationService();
                reservationService.UpdateLocation(latlng, _userId);
                //mMap.MarkerDragEnd += MMap_MarkerDragEnd;
            }
            else
            {
                Toast.MakeText(this.ApplicationContext, "Unable to determine the address. Try again in a few minutes.", ToastLength.Short).Show();
            }
        }
    }
}