namespace Trooper.Thorny.Business.Security
{
    using Interface.DataManager;
    using Operation.Core;
    using Response;
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Utility;

    public class Authorization<TPoco> : IAuthorization<TPoco>
        where TPoco : class
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
					OperationAction.AddAction, 
					OperationAction.AddSomeAction, 
					OperationAction.DeleteByKeyAction, 
					OperationAction.DeleteSomeByKeyAction,
					OperationAction.ExistsByKeyAction, 
					OperationAction.GetAllAction, 
					OperationAction.GetByKeyAction, 
					OperationAction.GetSomeByKeyAction, 
					OperationAction.GetSession, 
					OperationAction.GetSomeAction,
					OperationAction.IsAllowedAction, 
					OperationAction.UpdateAction,
					OperationAction.UpdateSomeAction,
                    OperationAction.SaveAction,
                    OperationAction.SaveSomeAction
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
            return action == OperationAction.AddAction
                || action == OperationAction.AddSomeAction;
        }

        public virtual bool IsRemoveDataAction(string action)
        {
            return action == OperationAction.DeleteByKeyAction
                || action == OperationAction.DeleteSomeByKeyAction;
        }

        public virtual bool IsUpdateAction(string action)
        {
            return action == OperationAction.UpdateAction
                || action == OperationAction.UpdateSomeAction;
        }

        public virtual bool IsSaveAction(string action)
        {
            return action == OperationAction.SaveAction
                || action == OperationAction.SaveSomeAction;
        }

        public virtual bool IsChangeAction(string action)
        {
            return this.IsAddDataAction(action)
                || this.IsUpdateAction(action)
                || this.IsSaveAction(action)
                || this.IsRemoveDataAction(action);
        }

        public virtual bool IsReadAction(string action)
        {
            return action == OperationAction.GetAllAction
                || action == OperationAction.GetSomeAction
                || action == OperationAction.GetByKeyAction
                || action == OperationAction.ExistsByKeyAction;
        }

        public bool IsValid(IIdentity identity)
        {
            var response = new Response();

            return this.IsValid(identity, response);
        }

        public virtual bool IsValid(IIdentity identity, IResponse response)
        {
            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity is invalid", BusinessCore.InvalidIdentityCode, response);
                return response.Ok;
            }

            var valid = !string.IsNullOrWhiteSpace(identity.Username) && !string.IsNullOrWhiteSpace(identity.Password);

            if (!valid)
            {
                MessageUtility.Errors.Add("The identity is invalid", BusinessCore.InvalidIdentityCode, response);
            }

            return valid;
        }

        public bool IsAllowed(IRequestArg<TPoco> arg, IIdentity identity)
        {
            return this.IsAllowed(arg, this.ResolveCredential(identity));
        }

        public bool IsAllowed(IRequestArg<TPoco> arg, ICredential credential)
        {
            var response = new Response();

            return this.IsAllowed(arg, credential, response);
        }

        public bool IsAllowed(IRequestArg<TPoco> arg, IIdentity identity, IResponse response)
        {
            return this.IsAllowed(arg, this.ResolveCredential(identity), response);
        }

        public virtual bool IsAllowed(IRequestArg<TPoco> arg, ICredential credential, IResponse response)
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
                    case OperationAction.AllActions:
                        behaviours.ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case OperationAction.AllAddActions:
                        behaviours.Where(action => this.IsAddDataAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case OperationAction.AllChangeActions:
                        behaviours.Where(action => this.IsChangeAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case OperationAction.AllUpdateActions:
                        behaviours.Where(action => this.IsUpdateAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case OperationAction.AllSaveActions:
                        behaviours.Where(action => this.IsSaveAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case OperationAction.AllReadActions:
                        behaviours.Where(action => this.IsReadAction(action.Action)).ToList()
                            .ForEach(action => action.Allow = behaviour.Allow);
                        break;
                    case OperationAction.AllRemoveActions:
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
                response.Messages = new List<Message>();

                MessageUtility.Errors.Add(
                    $"The user {credential.Username} is not allowed to perform action {arg.Action}.",
                    BusinessCore.UserDeniedCode,
                    null,
                    response);
            }

            return allowed;
        }
    }
}
