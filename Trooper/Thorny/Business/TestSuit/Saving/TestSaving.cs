namespace Trooper.Thorny.Business.TestSuit.Saving
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using Operation.Core;
    using System.Collections.Generic;

    public abstract class TestSaving<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<SavingRequirement<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();

                    foreach (var item in validItems)
                    {
                        requirement.Helper.MakeInvalidItem(item);

                        var response = requirement.Saver.Save(item, allowedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                    }
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();

                    foreach (var item in validItems)
                    {
                        requirement.Helper.MakeInvalidItem(item);

                        var response = requirement.Saver.Save(item, deniedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();

                    foreach (var item in validItems)
                    {
                        requirement.Helper.MakeInvalidItem(item);

                        var response = requirement.Saver.Save(item, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
                }

            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Saver.Save(invalidItem, allowedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Saver.Save(invalidItem, deniedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Saver.Save(invalidItem, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsAllowed_ReportsOkAndIsSavedAndOtherUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var itemToUpdate in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var baseItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl, new List<TPoco> { itemToUpdate }).ToList();
                        var state = requirement.Helper.AddItems(baseItems);
                        requirement.Helper.CopyIdentifiers(state.Last(), itemToUpdate);

                        var response = requirement.Saver.Save(itemToUpdate, allowedIdentity);
                        var newState = requirement.Helper.GetAllItems();
                        var otherStateItems = state.Where(i => !requirement.Helper.IdentifiersAreEqual(i, response.Item));
                        var otherNewStateItems = newState.Where(i => !requirement.Helper.IdentifiersAreEqual(i, response.Item));

                        Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                        Assert.That(response.Item, Is.Not.Null);
                        Assert.That(requirement.Helper.AreEqual(response.Item, itemToUpdate));
                        Assert.That(requirement.Helper.AreEqual(otherStateItems, otherNewStateItems), Is.True);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.False);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var itemToUpdate in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var baseItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl, new List<TPoco> { itemToUpdate }).ToList();
                        var state = requirement.Helper.AddItems(baseItems);
                        requirement.Helper.CopyIdentifiers(state.Last(), itemToUpdate);

                        var response = requirement.Saver.Save(itemToUpdate, deniedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var itemToUpdate in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var baseItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl, new List<TPoco> { itemToUpdate }).ToList();
                        var state = requirement.Helper.AddItems(baseItems);
                        requirement.Helper.CopyIdentifiers(state.Last(), itemToUpdate);

                        var response = requirement.Saver.Save(itemToUpdate, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsAllowed_ReportsOkAndIsSavedAndOtherUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var state = requirement.Helper.AddValidItems(newValidItem);
                        var response = requirement.Saver.Save(newValidItem, allowedIdentity);
                        var expected = new List<TPoco>(state);
                        expected.Add(response.Item);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(expected));
                        Assert.That(requirement.Helper.ResponseIsOk(response));
                        Assert.That(response.Item, Is.Not.Null);
                        Assert.That(requirement.Helper.NonIdentifiersAreEqual(newValidItem, response.Item));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var state = requirement.Helper.AddValidItems(newValidItem);
                        var response = requirement.Saver.Save(newValidItem, deniedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var state = requirement.Helper.AddValidItems(newValidItem);

                        var response = requirement.Saver.Save(newValidItem, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidExists_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.Gen))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(invalidItem, allowedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.Gen))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(invalidItem, deniedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.Gen))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(invalidItem, invalidIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidExists_IdentityIsAllowed_ReportsOkAndIsSaved()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(validItem, allowedIdentity);
                        var state = requirement.Helper.GetAllItems();

                        Assert.That(requirement.Helper.ResponseIsOk(response));
                        Assert.That(state.Count(), Is.EqualTo(1));
                        Assert.That(response.Item, Is.Not.Null);
                        Assert.That(requirement.Helper.NonIdentifiersAreEqual(response.Item, state.First()));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(validItem, deniedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(validItem, invalidIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(invalidItem, allowedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(invalidItem, deniedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(invalidItem, invalidIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsAllowed_ReportsOkAndIsSaved()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(validItem, allowedIdentity);
                        var state = requirement.Helper.GetAllItems();

                        Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                        Assert.That(state.Count(), Is.EqualTo(1));
                        Assert.That(response.Item, Is.Not.Null);
                        Assert.That(requirement.Helper.NonIdentifiersAreEqual(state.First(), response.Item));                        
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(validItem, deniedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Saver.Save(validItem, invalidIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }

        [Test]
        public void SelfTestHelper()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.SelfTestHelper();
            }
        }
    }
}
