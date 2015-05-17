using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
{
	using System.Collections.Generic;
	using System.Linq;
	using Trooper.Thorny.Interface.DataManager;
	using Trooper.Thorny.Interface.OperationResponse;
	using Trooper.Thorny.Utility;

    public class Authorization<Tc> : Authorization, IAuthorization<Tc> 
        where Tc : class,  new()
    {
        public IUnitOfWork Uow { get; set; }

        public virtual IList<IUserRole> Roles { get; set; }

        public virtual ICredential ResolveCredential(IIdentity identity)
        {
            return new Credential 
            { 
                Username = identity.Username,
                Session = identity.Session,
                Culture = identity.Culture
            };
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

	    public bool IsAllowed(IRequestArg<Tc> arg, IIdentity identity)
	    {
		    return this.IsAllowed(arg, this.ResolveCredential(identity));
	    }

        public bool IsAllowed(IRequestArg<Tc> arg, ICredential credential)
        {
            var response = new Response.Response();

            return this.IsAllowed(arg, credential, response);
        }

	    public bool IsAllowed(IRequestArg<Tc> arg, IIdentity identity, IResponse response)
	    {
		    return this.IsAllowed(arg, this.ResolveCredential(identity), response);
	    }

        public virtual bool IsAllowed(IRequestArg<Tc> arg, ICredential credential, IResponse response)
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
                    UserDeniedCode,
                    credential.Username,
                    arg.Action),
                null,
                response);
            }

            return false;
        }
    }

    public class Authorization
    {
        public const string UserDeniedCode = Constants.AuthorizationErrorCodeRoot + ".UserDenied";
    }
}
