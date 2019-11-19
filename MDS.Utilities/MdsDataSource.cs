using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using MdsWebService; 

namespace MDS.Utilities
{

    public sealed class MdsDataSource
    {

        private ServiceClient _client;


        public string MdsServiceLocation { get; set; }


        public MdsDataSource()
         : base()
        {
        }

        internal ServiceClient Client
        {
            get
            {
                //create the MDS proxy
                if (this.MdsServiceLocation == null || this.MdsServiceLocation == "")
                {
                    _client = new ServiceClient();  //use the web.config for the binding
                }
                else
                {
                    var binding = new BasicHttpBinding();
                    binding.MaxReceivedMessageSize = 1024 * 1024 * 5;
                    binding.SendTimeout = TimeSpan.FromMinutes(2);
                    binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;

                    var ep = new EndpointAddress(new Uri("http://m2bdbizagi01.minedu.gov.co/MDS/Service/Service.svc/bhb"));
                    _client = new ServiceClient(binding, ep);

                    _client.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

                    _client.ClientCredentials.Windows.ClientCredential.Domain = "M2BDBIZAGI01";
                    _client.ClientCredentials.Windows.ClientCredential.UserName = "serveradm";
                    _client.ClientCredentials.Windows.ClientCredential.Password = "Ministerio@2019";


                }
                return _client;
            }
             
        }


 




    }
}
