using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDS.Utilities;
using MdsWebService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSNivelAcademico.Models;

namespace WSNivelAcademico.Controllers
{

    [Produces("application/json")]
    [Route("nivelacademico/[controller]")]
    [ApiController]
    public class NivelAcademicoController : ControllerBase
    {

        private MDSEntity _entity { get; set; }
        private List<NivelAcademico> _listNivelAc { get; set; }


        #region Constantes

        private const string entidad = "NIVEL_ACADEMICO";
        private const string name = "Name";
        private const string code = "Code";
        private const string descripcion = "DESCRIPCION";
        private const string fechainicio = "FECHA_INICIO";
        private const string fechafin = "FECHA_FIN";
        private const string idniveleducativo = "ID_NIVEL_EDUCATIVO";

        #endregion


        public NivelAcademicoController()
        {
            this._entity = new MDSEntity(entidad);
           
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult("Datos de nivel academico");
        }

        [HttpPost]
        public List<NivelAcademico> Post(NivelAcademico academico = null)
        {
            string searchTerm = null;

            if (academico != null)
            {
                IDictionary<string, object> attributes =
                new Dictionary<string, object>();
                attributes.Add(code, academico.sCodigo);
                attributes.Add(name, academico.sNombre);
                attributes.Add(descripcion, academico.sDescripcion);
                attributes.Add(fechainicio, academico.dFechaInicio);
                attributes.Add(fechafin, academico.dFechaFin);

                searchTerm = this._entity.searchTermAttributes(attributes);
            }
             
            List<Member> result = this._entity.GetEntity(searchTerm);
            this._listNivelAc = new List<NivelAcademico>();
            foreach (Member member in result)
            {
                NivelAcademico  nivelAcademico  = new NivelAcademico();
                nivelAcademico.sNombre = member.MemberId.Name;
                nivelAcademico.sCodigo = member.MemberId.Code;
                nivelAcademico.sDescripcion = member.Attributes.Single(i => i.Identifier.Name.Equals(descripcion)).Value.ToString();

                var dInicio = member.Attributes.Single(i => i.Identifier.Name.Equals(fechainicio)).Value;
                var dFin= member.Attributes.Single(i => i.Identifier.Name.Equals(fechafin)).Value;
                nivelAcademico.dFechaInicio = dInicio == null ? null : (DateTime?)DateTime.Parse(dInicio.ToString());
                nivelAcademico.dFechaFin = dFin == null ? null : (DateTime?)DateTime.Parse(dFin.ToString());

               // nivelAcademico.idNivelEducativo = (decimal?) member.Attributes.Single(i => i.Identifier.Name.Equals(idniveleducativo)).Value.ToString();

                this._listNivelAc.Add(nivelAcademico);
            }

            return this._listNivelAc;
        }

        [Route("~/nivelacademico/add")]
        [HttpPost]
        public string Add(NivelAcademico nivelAcademico)
        {
            Member member = new Member();
            member.MemberId = new MemberIdentifier()
            {
                Name = nivelAcademico.sNombre,
                Code = nivelAcademico.sCodigo,
                MemberType = MemberType.Leaf,
            };
            IDictionary<string, object> attributes =
            new Dictionary<string, object>();
            attributes.Add(descripcion, nivelAcademico.sDescripcion);
            attributes.Add(fechainicio, nivelAcademico.dFechaInicio);
            attributes.Add(fechafin, nivelAcademico.dFechaFin);
            member.Attributes = new List<MdsWebService.Attribute>();
            member.Attributes = this._entity.setAttributes(attributes);
            var result = this._entity.AddMember(member);

            return result;
        }

        [Route("~/nivelacademico/update")]
        [HttpPost]
        public string Update(NivelAcademico nivelAcademico)
        {
            Member member = new Member();
            member.MemberId = new MemberIdentifier()
            {
                Name = nivelAcademico.sNombre,
                Code = nivelAcademico.sCodigo,
                MemberType = MemberType.Leaf,
            };
            IDictionary<string, object> attributes =
            new Dictionary<string, object>();
            attributes.Add(descripcion, nivelAcademico.sDescripcion);
            attributes.Add(fechainicio, nivelAcademico.dFechaInicio);
            attributes.Add(fechafin, nivelAcademico.dFechaFin);
            member.Attributes = new List<MdsWebService.Attribute>();
            member.Attributes = this._entity.setAttributes(attributes);
            var result = this._entity.UpdateMember(member);
            return result;
        }

        [Route("~/nivelacademico/delete")]
        [HttpPost]
        public string Delete(NivelAcademico nivelAcademico)
        {
            var result = this._entity.DeleteMember(nivelAcademico.sCodigo);
            return result;
        }



    }




}