using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurio.Client.Entities.Results;

namespace Neurio.Client.Tests
{
    public class TestBase
    {
        private NeurioClient _defaultClient = null;
        public NeurioClient DefaultClient
        {
            get
            {
                if (_defaultClient == null)
                {
                    _defaultClient = new NeurioClient()
                    {

                    };
                }
                return _defaultClient;
            }
        }


        public async Task<LoginResult> Login()
        {
            if (!DefaultClient.IsAuthenticated || DefaultClient.LastLoginResult == null)
            {
                return await DefaultClient.Login(System.Configuration.ConfigurationManager.AppSettings["Username"],
                    System.Configuration.ConfigurationManager.AppSettings["Password"]);
            }
            else
            {
                return DefaultClient.LastLoginResult;
            }
        }
    }
}
