
namespace Trooper.Thorny.Business.TestSuit.Selecting
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Operation.Core;
    using System.Collections.Generic;

    public abstract class TestGettingAll<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<SelectingRequirement<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_IdentityIsAllowed_ReportsOkAndIsSelectedAndUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();
                    
                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();
                    var response = requirement.Reader.GetAll(identity);                    

                    Assert.That(response.Items, Is.Not.Null);
                    Assert.That(requirement.Helper.AreEqual(response.Items, validItems));
                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseIsOk(response));                    
                }
            }
        }

        [Test]
        public virtual void HasItems_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var response = requirement.Reader.GetAll(identity);

                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var response = requirement.Reader.GetAll(identity);

                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_IdentityIsAllowed_ReportsOkAndIsSelected()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var response = requirement.Reader.GetAll(identity);

                    Assert.That(response.Items, Is.Not.Null);
                    Assert.That(response.Items, Is.Empty);
                    
                    Assert.That(requirement.Helper.ResponseIsOk(response));
                    Assert.That(requirement.Helper.HasNoItems());
                }
            }
        }

        [Test]
        public virtual void IsEmpty_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var response = requirement.Reader.GetAll(identity);

                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var response = requirement.Reader.GetAll(identity);

                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }
    }
}