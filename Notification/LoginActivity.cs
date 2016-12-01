using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Notification.Model;
using Notification.Service;

namespace Notification
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);
            // Create your application here

            Button loginSbmit = FindViewById<Button>(Resource.Id.loginBtn);
            loginSbmit.Click += LoginSbmit_Click;
        }

        private async void LoginSbmit_Click(object sender, EventArgs e)
        {
            EditText usernameTxt = FindViewById<EditText>(Resource.Id.usernameTxt);
            EditText passwordTxt = FindViewById<EditText>(Resource.Id.passwordTxt);

            var userlogin = new LoginModel
            {
                Username = usernameTxt.Text,
                Password = passwordTxt.Text
            };

            //var result = ApiCallService.CreateRequest(userlogin, Constants.ApiUrl + "user/login");
            //Task<HttpWebResponse> result = ApiCallService.CreateRequestAsync(userlogin, Constants.ApiUrl + "getuserid");
            var result = ApiCallService.CreateRequestAsync(userlogin, Constants.ApiUrl + "login/getuserid");
            var r = await result;
            // Will block until the task is completed...
            //HttpWebResponse result = response.GetAwaiter().GetResult();
            //HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            //var r = await result;

            using (var reader = new StreamReader(r.GetResponseStream()))
            {
                //Stream responseStream = response.GetResponseStream();
                //string responseStr = reader.ReadToEnd();
                //Console.WriteLine(responseStr);
                ////return XmlUtils.Deserialize<TResponse>(reader);

                string responseStr = reader.ReadToEnd();
                var user = JsonConvert.DeserializeObject<UserModel>(responseStr);
                //var user = JsonConvert.DeserializeObject<LoginViewModel>(responseStr);

                Context mContext = Application.Context;
                AppPreferences ap = new AppPreferences(mContext);

                //string key = "123123";
                ap.saveUserIdKey(user.UserId.ToString());
                //ap.saveUserIdKey(user.id.ToString());

                StartActivity(typeof(MainActivity));
            }

            //Context mContext = Application.Context;
            //AppPreferences ap = new AppPreferences(mContext);
            //ap.saveUserIdKey((5).ToString());

            //StartActivity(typeof(MainActivity));
        }
    }
}