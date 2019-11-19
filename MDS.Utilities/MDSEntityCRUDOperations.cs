using MdsWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MDS.Utilities
{
    public class MDSEntityCRUDOperations
    {

        protected readonly ServiceClient _client;

        protected string _ModelName { get; private set; }
        protected string _VersionName { get; private set; }
        protected string _EntityName { get; private set; }

        public MDSEntityCRUDOperations(ServiceClient client, string EntityName)
        {
            this._client = client;
            this._ModelName = "CONVALIDACIONES";
            this._VersionName = "VERSION_1";
            this._EntityName = EntityName;
        }


        public List<Member> GetMembers(string searchTerm)
        {
            EntityMembersGetCriteria criteria = new EntityMembersGetCriteria();
            criteria.ModelId = new Identifier() { Name = this._ModelName };
            criteria.EntityId = new Identifier() { Name = this._EntityName };
            criteria.VersionId = new Identifier() { Name = this._VersionName };
            criteria.MemberReturnOption = MemberReturnOption.DataAndCounts;

            EntityMembersGetRequest request = new EntityMembersGetRequest();
            request.International = new International();
            request.MembersGetCriteria = criteria;


            request.MembersGetCriteria.SearchTerm = searchTerm;

            EntityMembersGetResponse response = this._client.EntityMembersGetAsync(request).Result;

            if (response.OperationResult.Errors.Count() > 0)
            {
                var er = response.OperationResult.Errors.First();
                DataContractSerializer ds = new DataContractSerializer(er.GetType());
                var sw = new System.IO.StringWriter();
                var w = new System.Xml.XmlTextWriter(sw);

                ds.WriteObject(w, er);
                w.Flush();
                throw new InvalidOperationException(sw.ToString());
            }
            var members = new List<Member>();
            members = response.EntityMembers.Members;

            return members;
        }

        public string AddMember(Member member)
        {
            member.ValidationStatus = ValidationStatus.ValidationSucceeded;


            EntityMembers members = new EntityMembers();
            members.ModelId = new Identifier() { Name = this._ModelName };
            members.EntityId = new Identifier() { Name = this._EntityName };
            members.VersionId = new Identifier() { Name = this._VersionName };
            members.Members = new List<Member>();
            member.ValidationStatus = ValidationStatus.ValidationSucceeded;
            members.Members.Add(member);

            EntityMembersCreateRequest request = new EntityMembersCreateRequest();
            request.International = new International();
            request.Members = members;

            var response = this._client.EntityMembersCreateAsync(request).Result;
           
            if (response.OperationResult.Errors.Count > 0)
            {
                throw new InvalidOperationException(response.OperationResult.Errors[0].Description);
            }
            Publish(member.MemberId);
            return response.OperationResult.Errors.Count().ToString();
        }

        public string UpdateMember(Member member)
        {
            //Permiso de modificacion
            member.SecurityPermission = PermissionType.Admin;
            member.ValidationStatus = ValidationStatus.NewAwaitingValidation;

            EntityMembers members = new EntityMembers();
            members.ModelId = new Identifier() { Name = this._ModelName };
            members.EntityId = new Identifier() { Name = this._EntityName };
            members.VersionId = new Identifier() { Name = this._VersionName };
            members.Members = new List<Member>();
            members.Members.Add(member);

            EntityMembersUpdateRequest request = new EntityMembersUpdateRequest();
            request.International = new International();
            request.Members = members;

            var response = this._client.EntityMembersUpdateAsync(request).Result;
            if (response.OperationResult.Errors.Count > 0)
            {
                throw new InvalidOperationException(response.OperationResult.Errors[0].Description);
            }
            Publish(member.MemberId);
            return response.OperationResult.Errors.Count().ToString();
        }

        public string DeleteMember(string code)
        {
            var members = new EntityMembers() { Members = new List<Member>() };
            var member = new Member();
            member.MemberId = new MemberIdentifier() { Code = code };
            members.ModelId = new Identifier() { Name = this._ModelName };
            members.EntityId = new Identifier() { Name = this._EntityName };
            members.VersionId = new Identifier() { Name = this._VersionName };

            members.Members.Add(member);

            EntityMembersDeleteRequest request = new EntityMembersDeleteRequest();
            request.International = new International();
            request.Members = members;

            var response = this._client.EntityMembersDeleteAsync(request).Result;
            if (response.OperationResult.Errors.Count > 0)
            {
                throw new InvalidOperationException(response.OperationResult.Errors[0].Description);
            }
            return response.OperationResult.Errors.Count().ToString();
        }


        private string Publish(MemberIdentifier member)
        {

            member.MemberType = MemberType.Leaf;
            ValidationProcessRequest request = new ValidationProcessRequest();

            var members = new EntityMembers() { Members = new List<Member>() };
            members.ModelId = new Identifier() { Name = this._ModelName };
            members.EntityId = new Identifier() { Name = this._EntityName };
            members.VersionId = new Identifier() { Name = this._VersionName };

            request.International = new International();
            request.ValidationProcessCriteria = new ValidationProcessCriteria();
            request.ValidationProcessCriteria.EntityId = new Identifier() { Name = this._EntityName };
            request.ValidationProcessCriteria.ModelId = new Identifier() { Name = this._ModelName };
            request.ValidationProcessCriteria.VersionId = new Identifier() { Name = this._VersionName};

            request.ValidationProcessCriteria.Members = new List<MemberIdentifier>();
            request.ValidationProcessCriteria.Members.Add(member);

            request.ValidationProcessOptions =  new ValidationProcessOptions();
            request.ValidationProcessOptions.CommitVersion = false;
            request.ValidationProcessOptions.ReturnValidationResults = true;
            
            var response = this._client.ValidationProcessAsync(request).Result;

            if (response.OperationResult.Errors.Count > 0)
            {
                throw new InvalidOperationException(response.OperationResult.Errors[0].Description);
            }
            return response.OperationResult.Errors.Count().ToString();
        }



    }
}
