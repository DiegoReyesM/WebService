using System.Collections.Generic;
using System.Linq;
using MDS.Utilities;
using MdsWebService;
using Microsoft.AspNetCore.Mvc;
using WebServiceMDS.Models;

namespace WebServiceMDS.Controllers
{
    [Produces("application/json")]
    [Route("pais/[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult("Validación de País");
        }


        [HttpPost]
        public List<Pais> Post()
        {
            MDSEntity entity = new MDSEntity("PAIS");
            var result = entity.GetEntity("");
            List<Pais> paises = new List<Pais>();
            foreach( Member member in result)
            {
                Pais pais = new Pais();
                pais.sPais = member.Attributes.Single(i => i.Identifier.Name.Equals("PAIS")).Value.ToString();
                pais.sCodigo = member.Attributes.Single(i => i.Identifier.Name.Equals("ID_PAIS")).Value.ToString();

                paises.Add(pais);
            }
            return paises;
        }

        [Route("~/pais/add")]
        [HttpGet]
        public string Add()
        {
            Member member = new Member();
            member.MemberId = new MemberIdentifier()
            {
                Name = "Net",
                Code = "OK1",
                MemberType = MemberType.Leaf,


            };
            member.Attributes = new List<Attribute>();

            Attribute attribute = new Attribute();
            attribute.Value = "Pais Net";
            attribute.Type = AttributeValueType.String;
            attribute.Identifier = new Identifier() { Name = "PAIS" };
           // attribute.

           member.Attributes.Add(attribute);

            MDSEntity entity = new MDSEntity("PAIS");
            var result = entity.AddMember(member);
            IDictionary<string, object> values;

            MemberIdentifier memberIdentifier = new MemberIdentifier();

            memberIdentifier.Code = "OK1"; 


            return result;
        }

        [Route("~/pais/publish")]
        [HttpPost]
        public string Publish()
        {

            MemberIdentifier memberIdentifier = new MemberIdentifier();

            memberIdentifier.Code = "OK1";


            return "OK";
        }


        [Route("~/pais/update")]
        [HttpPost]
        public string Update()
        {
            Member member = new Member();
            member.MemberId = new MemberIdentifier()
            {
                Name = "Francia",
                Code = "10",
                MemberType = MemberType.Leaf,
            };
            member.Attributes = new List<Attribute>();

            Attribute attribute = new Attribute();
            attribute.Value = "Pais Inter 2";
            attribute.Type = AttributeValueType.String;
            attribute.Identifier = new Identifier() { Name = "PAIS" };
            // attribute.

            member.Attributes.Add(attribute);

            MDSEntity entity = new MDSEntity("PAIS");
            var result = entity.UpdateMember(member);

            return result;
        }



    }
}