namespace Trooper.BusinessOperation2.Business.Security
{
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.BusinessOperation2.Utility;

    public class Authorization<Tc> : IAuthorization<Tc> 
        where Tc : class,  new()
    {
        public IUnitOfWork Uow { get; set; }

        public IList<IUserRole> Roles { get; set; }

        public ICredential ResolveCredential(IIdentity identity)
        {
            return new Credential { Username = identity.Username };
        }

        public virtual bool IsAddDataAction(string action)
        {
            return action == Action.AddAction || action == Action.AddSomeAction;
        }

        public virtual bool IsUpdateDataAction(string action)
        {
            return action == Action.UpdateAction;
        }

        public virtual bool IsRemoveDataAction(string action)
        {
            return action == Action.DeleteByKeyAction
                || action == Action.DeleteSomeByKeyAction;
        }

        public virtual bool IsChangeAction(string action)
        {
            return this.IsAddDataAction(action)
                || this.IsUpdateDataAction(action)
                || this.IsRemoveDataAction(action);
        }

        public virtual bool IsReadAction(string action)
        {
            return action == Action.GetAllAction
                || action == Action.GetSomeAction
                || action == Action.GetByKeyAction
                || action == Action.ExistsByKeyAction;
        }

        public bool IsAllowed(IRequestArg<Tc> arg, ICredential credential)
        {
            var response = new Response.Response();

            return this.IsAllowed(arg, credential, response);
        }

        public virtual bool IsAllowed(IRequestArg<Tc> arg, ICredential credential, Interface.OperationResponse.IResponse response)
        {
            if (this.Roles == null)
            {
                return true;
            }

            var role = this.Roles.FirstOrDefault(ag => ag.Action == arg.Action);

            if (role == null && this.IsRemoveDataAction(arg.Action))
            {
                role = this.Roles.FirstOrDefault(ag => ag.Action == Action.AllRemoveActions);
            }

            if (role == null && this.IsChangeAction(arg.Action))
            {
                role = this.Roles.FirstOrDefault(ag => ag.Action == Action.AllChangeActions);
            }

            if (role == null && this.IsReadAction(arg.Action))
            {
                role = this.Roles.FirstOrDefault(ag => ag.Action == Action.AllReadActions);
            }

            if (role == null)
            {
                role = this.Roles.FirstOrDefault(ag => ag.Action == Action.AllActions);
            }

            //// No action found to check against so it's ok.
            if (role == null)
            {
                return true;
            }

            var hasUser = (role.Users != null && role.Users.Contains(credential.Username))
                          || (role.UserGroups != null && credential.Groups != null && role.UserGroups.Any(ug => credential.Groups.Contains(ug)));

            if (hasUser && role.Allow)
            {
                return true;
            }

            if (response != null)
            {
                MessageUtility.Errors.Add(
                string.Format(
                    "The user {0} is not allowed to perform action {1}.",
                    credential.Username,
                    arg.Action),
                null,
                response);
            }

            return false;
        }
    }
}
