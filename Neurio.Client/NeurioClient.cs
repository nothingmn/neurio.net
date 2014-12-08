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
        public static string AlphaSiteBaseUrl = "https://alpha.neur.io/v1";
        private readonly string _baseUrl;

        public NeurioClient(string baseUrl = null)
        {
            _baseUrl = baseUrl;
            if (string.IsNullOrEmpty(_baseUrl)) _baseUrl = AlphaSiteBaseUrl;
            if (_baseUrl.EndsWith("/")) _baseUrl = _baseUrl.Substring(0, _baseUrl.Length - 1);
        }

        public async Task<AppliancesResult> Appliances(string locationId)
        {
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
        public async Task<List<Event>> LocationEvents(string locationId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, int page = 1, int perPage = 100 )
        {
            try
            {
                if (startDateTime == DateTimeOffset.MinValue) startDateTime = DateTimeOffset.Now.AddDays(-7);
                if (endDateTime == DateTimeOffset.MaxValue) endDateTime = DateTimeOffset.Now;
                if (page < 1) page = 1;
                if (perPage < 1) perPage = 10;
                return await HttpClient.GetAsType<List<Event>>(FormatUrl("appliances/events", string.Format("locationId={0}&start={1}&end={2}&page={3}&perPage={4}", locationId, FormatDateTimeOffset(startDateTime.UtcDateTime), FormatDateTimeOffset(endDateTime.UtcDateTime), page, perPage)));
            }
            catch (Exception)
            {
            }
            return null;
        }
        public async Task<List<Stat>> LocationStats(string locationId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, int page = 1, int perPage = 100, string granularity = "days")
        {
            try
            {
                if (startDateTime == DateTimeOffset.MinValue) startDateTime = DateTimeOffset.Now.AddDays(-7);
                if (endDateTime == DateTimeOffset.MaxValue) endDateTime = DateTimeOffset.Now;
                if (page < 1) page = 1;
                if (perPage < 1) perPage = 10;
                return await HttpClient.GetAsType<List<Stat>>(FormatUrl("appliances/stats", string.Format("locationId={0}&start={1}&end={2}&page={3}&perPage={4}&granularity={5}", locationId, FormatDateTimeOffset(startDateTime.UtcDateTime), FormatDateTimeOffset(endDateTime.UtcDateTime), page, perPage, granularity)));
            }
            catch (Exception)
            {
            }
            return null;
        }

        public async Task<List<Sample>> SensorLiveSamples(string sensorId, DateTimeOffset lastDateTime)
        {
            try
            {
                if (lastDateTime == DateTimeOffset.MinValue) lastDateTime = DateTimeOffset.Now;
                return await HttpClient.GetAsType<List<Sample>>(FormatUrl("samples/live", string.Format("sensorId={0}&last=", sensorId, FormatDateTimeOffset(lastDateTime.UtcDateTime))));
            }
            catch (Exception)
            {
            }
            return null;
        }


        public async Task<List<Sample>> SensorStatsSamples(string sensorId, DateTimeOffset startDateTime, DateTimeOffset endDateTime, string granularity = "days")
        {
            try
            {
                if (string.IsNullOrEmpty(granularity)) granularity = "days";
                if (startDateTime == DateTimeOffset.MinValue) startDateTime = DateTimeOffset.Now.AddDays(-7);
                if (endDateTime == DateTimeOffset.MaxValue) endDateTime = DateTimeOffset.Now;
                return await HttpClient.GetAsType<List<Sample>>(FormatUrl("samples/stats", string.Format("sensorId={0}&granularity={1}&start={2}&end={3}", sensorId, granularity, FormatDateTimeOffset(startDateTime.UtcDateTime), FormatDateTimeOffset(endDateTime.UtcDateTime))));
            }
            catch (Exception)
            {
            }
            return null;
        }

        public async Task<CurrentUserResult> CurrentUser()
        {
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
            return user;
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

            return loginResult;
        }
        private async Task<T> GetResourceAsType<T>(string resource, string queryString = null)
        {
            
            Debug.WriteLine("GetResourceAsType {0}, {1}", resource, queryString);
            var result = await HttpClient.GetAsType<T>(FormatUrl(resource, queryString));
            Debug.WriteLine("GetResourceAsType {0}, {1} returned", resource, queryString);
            return result;
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
