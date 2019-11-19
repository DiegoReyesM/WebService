using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDS.Utilities;
using MdsWebService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSNivelEducativo.Models;

namespace WSNivelEducativo.Controllers
{
    [Produces("application/json")]
    [Route("niveleducativo/[controller]")]
    [ApiController]
    public class NivelEducativoController : ControllerBase
    {
        private MDSEntity _entity { get; set; }
        private List<NivelEducativo> _listNivelAc { get; set; }


        #region Constantes

        private const string entidad = "NIVEL_EDUCATIVO";
        private const string name = "Name";
        private const string code = "Code";
        private const string descripcion = "DESCRIPCION";
        private const string fechainicio = "FECHA_INICIO";
        private const string fechafin = "FECHA_FIN";

        #endregion


        public NivelEducativoController()
        {
            this._entity = new MDSEntity(entidad);
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult("Datos de nivel educativo");
        }

        [HttpPost]
        public List<NivelEducativo> Post(NivelEducativo educativo = null)
        {
            string searchTerm = null;

            if (educativo != null)
            {
                IDictionary<string, object> attributes =
                new Dictionary<string, object>();
                attributes.Add(code, educativo.sCodigo);
                attributes.Add(name, educativo.sNombre);
                attributes.Add(descripcion, educativo.sDescripcion);
                attributes.Add(fechainicio, educativo.dFechaInicio);
                attributes.Add(fechafin, educativo.dFechaFin);

                searchTerm = this._entity.searchTermAttributes(attributes);
            }

            List<Member> result = this._entity.GetEntity(searchTerm);
            this._listNivelAc = new List<NivelEducativo>();
            foreach (Member member in result)
            {
                NivelEducativo nivelEducativo = new NivelEducativo();
                nivelEducativo.sNombre = member.MemberId.Name;
                nivelEducativo.sCodigo = member.MemberId.Code;
                nivelEducativo.sDescripcion = member.Attributes.Single(i => i.Identifier.Name.Equals(descripcion)).Value.ToString();

                var dInicio = member.Attributes.Single(i => i.Identifier.Name.Equals(fechainicio)).Value;
                var dFin = member.Attributes.Single(i => i.Identifier.Name.Equals(fechafin)).Value;
                nivelEducativo.dFechaInicio = dInicio == null ? null : (DateTime?)DateTime.Parse(dInicio.ToString());
                nivelEducativo.dFechaFin = dFin == null ? null : (DateTime?)DateTime.Parse(dFin.ToString());

                this._listNivelAc.Add(nivelEducativo);
            }

            return this._listNivelAc;
        }

        [Route("~/niveleducativo/add")]
        [HttpPost]
        public string Add(NivelEducativo nivelEducativo)
        {
            Member member = new Member();
            member.MemberId = new MemberIdentifier()
            {
                Name = nivelEducativo.sNombre,
                Code = nivelEducativo.sCodigo,
                MemberType = MemberType.Leaf,
            };
            IDictionary<string, object> attributes =
            new Dictionary<string, object>();
            attributes.Add(descripcion, nivelEducativo.sDescripcion);
            attributes.Add(fechainicio, nivelEducativo.dFechaInicio);
            attributes.Add(fechafin, nivelEducativo.dFechaFin);
            member.Attributes = new List<MdsWebService.Attribute>();
            member.Attributes = this._entity.setAttributes(attributes);
            var result = this._entity.AddMember(member);

            return result;
        }


        [Route("~/niveleducativo/update")]
        [HttpPost]
        public string Update(NivelEducativo nivelEducativo)
        {
            Member member = new Member();
            member.MemberId = new MemberIdentifier()
            {
                Name = nivelEducativo.sNombre,
                Code = nivelEducativo.sCodigo,
                MemberType = MemberType.Leaf,
            };
            IDictionary<string, object> attributes =
            new Dictionary<string, object>();
            attributes.Add(descripcion, nivelEducativo.sDescripcion);
            attributes.Add(fechainicio, nivelEducativo.dFechaInicio);
            attributes.Add(fechafin, nivelEducativo.dFechaFin);
            member.Attributes = new List<MdsWebService.Attribute>();
            member.Attributes = this._entity.setAttributes(attributes);
            var result = this._entity.UpdateMember(member);
            return result;
        }

        [Route("~/niveleducativo/delete")]
        [HttpPost]
        public string Delete(NivelEducativo nivelEducativo)
        {
            var result = this._entity.DeleteMember(nivelEducativo.sCodigo);
            return result;
        }

    }
}