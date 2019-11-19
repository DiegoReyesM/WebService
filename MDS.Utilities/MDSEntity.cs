using MdsWebService;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Utilities
{
    public class MDSEntity
    {
        private MdsDataSource owner;

        protected string _EntityName { get; private set; }

        protected MDSEntityCRUDOperations operations { get; private set; }

        public MDSEntity(string EntityName)
        {
            this._EntityName = EntityName;
            owner = new MdsDataSource();
            operations = new MDSEntityCRUDOperations(this.owner.Client, this._EntityName);
        }


        public List<Member> GetEntity(string searchTerm)
        {
            var result = operations.GetMembers(searchTerm);
            return result;
        }

        public string AddMember(Member member)
        {
            var result = operations.AddMember(member);
            return result;
        }

        public string UpdateMember(Member member)
        {
            var result = operations.UpdateMember(member);
            return result;
        }

        public string DeleteMember(string code)
        {
            var result = operations.DeleteMember(code);
            return result;
        }

        public List<MdsWebService.Attribute> setAttributes(IDictionary<string, object> keyValues)
        {
            List<MdsWebService.Attribute> listAtribute = new List<MdsWebService.Attribute>();

            foreach (var value in keyValues)
            {
                if (value.Value != null && value.Value.ToString() != string.Empty && value.Value.ToString() != "0")
                {
                    listAtribute.Add(new MdsWebService.Attribute() { Identifier = new Identifier() { Name = value.Key }, Value = value.Value });
                }
            }
            return listAtribute;
        }

        public string searchTermAttributes(IDictionary<string, object> keyValues)
        {
            List<MdsWebService.Attribute> attributes = new List<MdsWebService.Attribute>(); 
            attributes = setAttributes(keyValues);
            string searchTerm = string.Empty;
            int and = 0;
            foreach (var value in attributes)
            { 
                if (and > 0 )
                    searchTerm += " AND ";

                if (value.Identifier.Name == "Code")
                    searchTerm += String.Format(" {0} = '{1}' ", value.Identifier.Name, value.Value);
                else
                    searchTerm += String.Format("{0} like '{1}' ", value.Identifier.Name, value.Value);
                and++;
            }
            return searchTerm;
        }

    }


}
