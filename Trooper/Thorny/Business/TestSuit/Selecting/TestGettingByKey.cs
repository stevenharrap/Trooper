
namespace Trooper.Thorny.Business.TestSuit.Selecting
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Operation.Core;
    using System.Collections.Generic;

    public abstract class TestGettingByKey<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<SelectingRequirement<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsAllowed_ReportsOkAndIsSelectedAndOtherUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();
                var items = requirement.Helper.AddValidItems().ToList();
                var state = requirement.Helper.GetAllItems();
                requirement.Helper.MakeInvalidItems(items);

                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                    foreach (var item in items)
                    {
                        var response = requirement.Reader.GetByKey(item, identity);

                        Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                        Assert.That(response.Item, Is.Not.Null);
                        Assert.That(requirement.Helper.AreEqual(item, response.Item));
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();
                var items = requirement.Helper.AddValidItems().ToList();
                var state = requirement.Helper.GetAllItems();
                requirement.Helper.MakeInvalidItems(items);

                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                    foreach (var item in items)
                    {
                        var response = requirement.Reader.GetByKey(item, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();
                var items = requirement.Helper.AddValidItems().ToList();
                var state = requirement.Helper.GetAllItems();
                requirement.Helper.MakeInvalidItems(items);

                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                    foreach (var item in items)
                    {
                        var response = requirement.Reader.GetByKey(item, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsInvalid_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsInvalidNotExists_IdentityIsAllowed_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsInvalidNotExists_IdentityIsDenied_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsInvalidNotExists_IdentityIsInvalid_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsAllowed_ReportsOkAndIsSelectedAndOtherUnchanged() { }
        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsDenied_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsInvalid_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsAllowed_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsInvalid_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsValidNotExists_IdentityIsAllowed_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsValidNotExists_IdentityIsDenied_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void HasItems_ItemIsValidNotExists_IdentityIsInvalid_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsInvalid_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsAllowed_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange() { }
        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsInvalid_ReportsErrorAndNoChange() { }
    }
}