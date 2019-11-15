using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMDS;
using System.Security.Principal;
using WebServiceMDS.Models;

namespace WebServiceMDS.Controllers
{
    [Route("WebService/[controller]")]
    [ApiController]
    public class MDSController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 1024 * 1024 * 5;
            binding.SendTimeout = TimeSpan.FromMinutes(2);
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;

            var ep = new EndpointAddress(new Uri("http://m2bdbizagi01.minedu.gov.co/MDS/Service/Service.svc/bhb"));

            ServiceClient client = new ServiceClient(binding, ep);
            client.ClientCredentials.Windows.AllowedImpersonationLevel =
            TokenImpersonationLevel.Impersonation;

            client.ClientCredentials.Windows.ClientCredential.Domain = "M2BDBIZAGI01";
            client.ClientCredentials.Windows.ClientCredential.UserName = "serveradm";
            client.ClientCredentials.Windows.ClientCredential.Password = "Ministerio@2019";

            EntityMembersGetRequest request = new EntityMembersGetRequest();
            request.International = new International();
            request.MembersGetCriteria = new EntityMembersGetCriteria();
            request.MembersGetCriteria.MemberReturnOption = MemberReturnOption.Data;
            request.MembersGetCriteria.ModelId = new Identifier() { Name = "CONVALIDACIONES" };
            request.MembersGetCriteria.VersionId = new Identifier() { Name = "VERSION_1" };
            request.MembersGetCriteria.EntityId = new Identifier() { Name = "PAIS" };
            request.MembersGetCriteria.MemberType = MemberType.Leaf;
            // Get the entity member information
            EntityMembersGetResponse getResponse = client.EntityMembersGet(request);

            Pais pais = new Pais();
            // pais.sPais = getResponse.EntityMembers.Members.ToList()[0].Attributes[0].Value.ToString();

            // pais.sPais = getResponse.EntityMembers.Members.ToList()[0].Attributes.Where(x => x.Identifier.Name.All(y => y. ));

            pais.sPais = getResponse.EntityMembers.Members[0].Attributes
                .Where(i => i.Identifier.Name.Equals("PAIS"))
                .FirstOrDefault().Value.ToString();

            /*var fechaInicio = getResponse.EntityMembers.Members[0].Attributes
                .Where(i => i.Identifier.Name.Equals("FECHA_INICIO"))
                .FirstOrDefault().Value.ToString();

            var fechaFin = getResponse.EntityMembers.Members[0].Attributes
                .Where(i => i.Identifier.Name.Equals("FECHA_FIN"))
                .FirstOrDefault().Value.ToString();*/

          //  pais.dFechaInicio = true == true ? Convert.ToDateTime(fechaInicio) : null;  
           // pais.dFechaFin = Condition == true ? Convert.ToDateTime(fechaFin) : null;


            JsonResult response = new JsonResult(getResponse);

            return   new JsonResult(getResponse);
        }



    }
}