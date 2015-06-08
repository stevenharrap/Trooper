using System.Web.WebPages;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
{
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Thorny.Interface.OperationResponse;
    using Trooper.Thorny.Utility;
    using Trooper.Thorny.Business.Operation.Core;

    public class Authorization<Tc> : IAuthorization<Tc>
        where Tc : class,  new()
    {
        public IUnitOfWork Uow { get; set; }

        public virtual IList<IAssignment> Assignments
        {
            get { return null; }
        }

        public virtual IList<string> AllActions
        {
            get
            {
                return new[]
				{
					Action.AddAction, 
					Action.AddSomeAction, 
					Action.DeleteByKeyAction, 
					Action.DeleteSomeByKeyAction,
					Action.ExistsByKeyAction, 
					Action.GetAllAction, 
					Action.GetByKeyAction, 
					Action.GetSomeByKeyAction, 
					Action.GetSession, 
					Action.GetSomeAction,
					Action.IsAllowedAction, 
					Action.UpdateAction,
					Action.UpdateSomeAction
				};
            }
        }

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
            return action == Action.AddAction
                || action == Action.AddSomeAction;
        }

        public virtual bool IsRemoveDataAction(string action)
        {
            return action == Action.DeleteByKeyAction
                || action == Action.DeleteSomeByKeyAction;
        }

        public virtual bool IsUpdateAction(string action)
        {
            return action == Action.UpdateAction
                || action == Action.UpdateSomeAction;
        }

        public virtual bool IsChangeAction(string action)
        {
            return this.IsAddDataAction(action)
                || this.IsUpdateAction(action)
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
            if (this.Assignments == null)
            {
                return true;
            }

            var behaviours = this.AllActions.Select(a => new Behaviour { Action = a, Allow = false }).ToList();
            var userAssignments = from a in this.Assignments
                                  where (a.UserGroups != null && a.UserGroups.Any(ug => ug == credential.Username)) ||
                                        (a.Users != null && a.Users.Any(u => u == credential.Username))
                                  orderby a.Precedence ascending
                                  select a;

            foreach (var behaviour in userAssignments.SelectMany(assignment => assignment.Role))
            {
                switch (behaviour.Action)
                {
                    case Action.AllActions:
                        behaviours.ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case Action.AllAddActions:
                        behaviours.Where(action => this.IsAddDataAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case Action.AllChangeActions:
                        behaviours.Where(action => this.IsChangeAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case Action.AllUpdateActions:
                        behaviours.Where(action => this.IsUpdateAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case Action.AllReadActions:
                        behaviours.Where(action => this.IsReadAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case Action.AllRemoveActions:
                        behaviours.Where(action => this.IsRemoveDataAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    default:
                        behaviours.Where(action => action.Action == behaviour.Action).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                }
            }

            var allowed = behaviours.Any(behaviour => behaviour.Action == arg.Action && behaviour.Allow);

            if (!allowed && response != null)
            {
                response.Messages = new List<IMessage>();

                MessageUtility.Errors.Add(
                string.Format(
                    "The user {0} is not allowed to perform action {1}.",
                    credential.Username,
                    arg.Action),
                BusinessCore.UserDeniedCode,
                null,
                response);
            }

            return allowed;
        }
    }
}
