using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Neurio.Client.Entities;
using Neurio.Client.Entities.Results;
using Neurio.Client.Extensions;

namespace Neurio.Client
{
    public class NeurioClient
    {
        public CurrentUserResult User { get; private set; }
        public LoginResult LastLoginResult { get; private set; }

        public static string AlphaSiteBaseUrl = "https://alpha.neur.io/v1";
        private readonly string _baseUrl;

        public NeurioClient(string baseUrl = null)
        {
            _baseUrl = baseUrl;
            if (string.IsNullOrEmpty(_baseUrl)) _baseUrl = AlphaSiteBaseUrl;
            if (_baseUrl.EndsWith("/")) _baseUrl = _baseUrl.Substring(0, _baseUrl.Length - 1);
        }

        public async Task<AppliancesResult> LoadAppliances(string locationId)
        {
            if (!IsAuthenticated) return null;

            var appliances = new AppliancesResult()
            {
                StatusCode = -1,
                Success = false
            };

            try
            {
                var list  = await HttpClient.GetAsType<List<Appliance>>(FormatUrl("appliances", string.Format("locationId={0}", locationId)));
                appliances.Appliances = list;
                appliances.Success = true;
                appliances.StatusCode = 200;
                this.User.Appliances = list;

            }
            catch (Exception e)
            {
                appliances.Message = e.ToString();
            }
            return appliances;
        }

        private string FormatDateTimeOffset(DateTimeOffset dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        public async Task<List<Event>> LoadLocationEvents(string locationId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, int page = 1, int perPage = 100 )
        {
            try
            {
                if (!IsAuthenticated) return null;

                if (startDateTime == DateTimeOffset.MinValue) startDateTime = DateTimeOffset.Now.AddDays(-7);
                if (endDateTime == DateTimeOffset.MaxValue) endDateTime = DateTimeOffset.Now;
                if (page < 1) page = 1;
                if (perPage < 1) perPage = 10;
                var list = await HttpClient.GetAsType<List<Event>>(FormatUrl("appliances/events", string.Format("locationId={0}&start={1}&end={2}&page={3}&perPage={4}", locationId, FormatDateTimeOffset(startDateTime.UtcDateTime), FormatDateTimeOffset(endDateTime.UtcDateTime), page, perPage)));


                foreach (var l in User.Locations.Where(l => l.Id == locationId))
                {
                    l.Events = list;
                }

                return list;

            }
            catch (Exception)
            {
            }
            return null;
        }
        public async Task<List<Stat>> LoadLocationStats(string locationId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, int page = 1, int perPage = 100, string granularity = "days")
        {
            try
            {
                if (!IsAuthenticated) return null;

                if (startDateTime == DateTimeOffset.MinValue) startDateTime = DateTimeOffset.Now.AddDays(-7);
                if (endDateTime == DateTimeOffset.MaxValue) endDateTime = DateTimeOffset.Now;
                if (page < 1) page = 1;
                if (perPage < 1) perPage = 10;
                var list = await HttpClient.GetAsType<List<Stat>>(FormatUrl("appliances/stats", string.Format("locationId={0}&start={1}&end={2}&page={3}&perPage={4}&granularity={5}", locationId, FormatDateTimeOffset(startDateTime.UtcDateTime), FormatDateTimeOffset(endDateTime.UtcDateTime), page, perPage, granularity)));

                foreach (var l in User.Locations.Where(l => l.Id == locationId))
                {
                    l.Stats = list;
                }

                return list;

            }
            catch (Exception)
            {
            }
            return null;
        }

        public async Task<List<Sample>> LoadSensorLiveSamples(string sensorId, DateTimeOffset lastDateTime)
        {
            try
            {
                if (!IsAuthenticated) return null;

                if (lastDateTime == DateTimeOffset.MinValue) lastDateTime = DateTimeOffset.Now;
                var list = await HttpClient.GetAsType<List<Sample>>(FormatUrl("samples/live", string.Format("sensorId={0}&last={1}", sensorId, FormatDateTimeOffset(lastDateTime.UtcDateTime))));

                foreach (var s in from l in this.User.Locations from s in l.Sensors where s.Id == sensorId select s)
                {
                    s.LiveSamples = list;
                }

                return list;
            }
            catch (Exception)
            {
            }
            return null;
        }


        public async Task<List<Sample>> LoadSensorStatsSamples(string sensorId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, string granularity = "days")
        {
            try
            {
                if (!IsAuthenticated) return null;

                if (string.IsNullOrEmpty(granularity)) granularity = "days";
                if (startDateTime == DateTimeOffset.MinValue) startDateTime = DateTimeOffset.Now.AddDays(-7);
                if (endDateTime == DateTimeOffset.MaxValue) endDateTime = DateTimeOffset.Now;
                var list = await HttpClient.GetAsType<List<Sample>>(FormatUrl("samples/stats", string.Format("sensorId={0}&granularity={1}&start={2}&end={3}", sensorId, granularity, FormatDateTimeOffset(startDateTime.UtcDateTime), FormatDateTimeOffset(endDateTime.UtcDateTime))));

                foreach (var s in from l in this.User.Locations from s in l.Sensors where s.Id == sensorId select s)
                {
                    s.Samples = list;
                }

                return list;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public bool IsAuthenticated { get; private set; }
        public async Task<CurrentUserResult> LoadCurrentUser()
        {
            if (!IsAuthenticated) return null;

            CurrentUserResult user = null;
            try
            {
                user = await HttpClient.GetAsType<CurrentUserResult>(FormatUrl("users/current"));                
            }
            catch (Exception e)
            {
                user = new CurrentUserResult()
                {
                    Success = false,
                    StatusCode = -1,
                    Message = e.ToString()
                };
                
            }
            this.User = user;
            return user;
        }


        public async Task<bool> Logout()
        {
            if (!IsAuthenticated) return false;
            _client = null;
            return true;
        }
        public async Task<LoginResult> Login(string username, string password)
        {
            _client = null;
            LoginResult loginResult = null;
            try
            {
                
                loginResult =
                    await
                        HttpClient.PostAsType<LoginResult>(FormatUrl("oauth2/token"),
                            string.Format("grant_type=password&username={0}&password={1}", Uri.EscapeUriString(username), Uri.EscapeUriString(password)),
                            "application/x-www-form-urlencoded");
                IsAuthenticated = loginResult.Success;
                if (loginResult.Success)
                {
                    if (!string.IsNullOrEmpty(loginResult.Access_token) && loginResult.Success)
                    {
                        _client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", loginResult.Access_token));
                        //_client.DefaultRequestHeaders.Add("zvstoken", loginResult.zvstoken);
                    }
                }


            }
            catch (Exception e)
            {
                loginResult = new LoginResult()
                {
                  
                };
            }
            LastLoginResult = loginResult;

            return loginResult;
        }

        private string ContentType = "application/json";

        private string FormatUrl(string resource, string queryString = null)
        {
            var url = string.Format("{0}/{1}", _baseUrl, resource);
            if (!string.IsNullOrEmpty(queryString))
            {
                url = string.Format("{0}?{1}", url, queryString);
            }
            return url;
        }

        private HttpClientHandler handler = new HttpClientHandler();
        private HttpClient _client = null;
        private object _clientLock = new object();

        private HttpClient HttpClient
        {
            get
            {
                lock (_clientLock)
                {
                    if (_client == null)
                    {
                        handler.CookieContainer = new CookieContainer();
                        _client = new HttpClient();
                        //_client.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
                        //_client.DefaultRequestHeaders.Add("Content-Type", ContentType);

                    }
                }
                return _client;
            }
        }
    }
}
