
namespace Trooper.Thorny.Business.TestSuit.Adding
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit;
    using Operation.Core;

    public abstract class Adding<TPoco>
        where TPoco : class
    {
        public abstract Func<AddingRequirment<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsAllowed_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var identity = requirement.Helper.MakeAllowedIdentity();
                var item3 = requirement.Helper.MakeInvalidItem();
                requirement.Helper.CopyIdentifiers(item2, item3);

                requirement.Creater.Add(item3, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsAllowed_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeAllowedIdentity();
                var item3 = requirement.Helper.MakeInvalidItem();
                requirement.Helper.CopyIdentifiers(item2, item3);

                var response = requirement.Creater.Add(item3, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsDenied_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var identity = requirement.Helper.MakeDeniedIdentity();
                var item3 = requirement.Helper.MakeInvalidItem();
                requirement.Helper.CopyIdentifiers(item2, item3);

                requirement.Creater.Add(item3, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsDenied_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeDeniedIdentity();
                var item3 = requirement.Helper.MakeInvalidItem();
                requirement.Helper.CopyIdentifiers(item2, item3);

                var response = requirement.Creater.Add(item3, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsInvalid_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var identity = requirement.Helper.MakeInvalidIdentity();
                var item3 = requirement.Helper.MakeInvalidItem();
                requirement.Helper.CopyIdentifiers(item2, item3);

                requirement.Creater.Add(item3, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsInvalid_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeInvalidIdentity();
                var item3 = requirement.Helper.MakeInvalidItem();
                requirement.Helper.CopyIdentifiers(item2, item3);

                var response = requirement.Creater.Add(item3, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsNull_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeInvalidItem();
                requirement.Helper.CopyIdentifiers(item2, item3);

                requirement.Creater.Add(item3, null);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsNull_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var item3 = requirement.Helper.MakeInvalidItem();
                requirement.Helper.CopyIdentifiers(item2, item3);

                var response = requirement.Creater.Add(item3, null);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsAllowed_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeAllowedIdentity();                

                requirement.Creater.Add(item3, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsAllowed_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var item3 = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeAllowedIdentity();

                var response = requirement.Creater.Add(item3, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsDenied_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeDeniedIdentity();

                requirement.Creater.Add(item3, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsDenied_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var item3 = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeDeniedIdentity();

                var response = requirement.Creater.Add(item3, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsInvalid_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                requirement.Creater.Add(item3, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsInvalid_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var item3 = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                var response = requirement.Creater.Add(item3, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsNull_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                requirement.Creater.Add(item3, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsNull_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var item3 = requirement.Helper.MakeInvalidItem();

                var response = requirement.Creater.Add(item3, null);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsNull_IdentityIsAllowed_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var identity = requirement.Helper.MakeAllowedIdentity();

                requirement.Creater.Add(null, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsNull_IdentityIsAllowed_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeAllowedIdentity();

                var response = requirement.Creater.Add(null, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsNull_IdentityIsDenied_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var identity = requirement.Helper.MakeDeniedIdentity();

                requirement.Creater.Add(null, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsNull_IdentityIsDenied_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeDeniedIdentity();

                var response = requirement.Creater.Add(null, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsNull_IdentityIsInvalid_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var identity = requirement.Helper.MakeInvalidIdentity();

                requirement.Creater.Add(null, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsNull_IdentityIsInvalid_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                var response = requirement.Creater.Add(null, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsNull_IdentityIsNull_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();

                requirement.Creater.Add(null, null);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsNull_IdentityIsNull_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();

                var response = requirement.Creater.Add(null, null);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsAllowed_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();                
                var identity = requirement.Helper.MakeAllowedIdentity();
                requirement.Helper.ChangeNonIdentifiers(item2);

                requirement.Creater.Add(item2, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsAllowed_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeAllowedIdentity();
                requirement.Helper.ChangeNonIdentifiers(item2);

                var response = requirement.Creater.Add(item2, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.AddFailedCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsDenied_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var identity = requirement.Helper.MakeDeniedIdentity();
                requirement.Helper.ChangeNonIdentifiers(item2);

                requirement.Creater.Add(item2, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsDenied_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeDeniedIdentity();
                requirement.Helper.ChangeNonIdentifiers(item2);

                var response = requirement.Creater.Add(item2, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsInvalid_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var identity = requirement.Helper.MakeInvalidIdentity();
                requirement.Helper.ChangeNonIdentifiers(item2);

                requirement.Creater.Add(item2, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsInvalid_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeInvalidIdentity();
                requirement.Helper.ChangeNonIdentifiers(item2);

                var response = requirement.Creater.Add(item2, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsNull_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                requirement.Helper.ChangeNonIdentifiers(item2);

                requirement.Creater.Add(item2, null);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsNull_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                requirement.Helper.ChangeNonIdentifiers(item2);

                var response = requirement.Creater.Add(item2, null);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsAllowed_IsAdded()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeAllowedIdentity();
                var item3 = requirement.Helper.MakeValidItem();

                var response = requirement.Creater.Add(item3, identity);

                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Item);
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(new TPoco[] { item1, item2, response.Item }));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsAllowed_OthersUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeAllowedIdentity();               
                var item3 = requirement.Helper.MakeValidItem();
                requirement.Helper.ChangeNonIdentifiers(item3);

                var response = requirement.Creater.Add(item3, identity);

                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Item);
                Assert.That(requirement.Helper.AreEqual(item1, requirement.Helper.GetItem(item1)));
                Assert.That(requirement.Helper.AreEqual(item2, requirement.Helper.GetItem(item2)));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsAllowed_ReportsOk()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var identity = requirement.Helper.MakeAllowedIdentity();
                var item3 = requirement.Helper.MakeValidItem();

                var response = requirement.Creater.Add(item3, identity);

                Assert.IsNotNull(response);
                Assert.That(response.Ok);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsDenied_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();                
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeDeniedIdentity();

                requirement.Creater.Add(item3, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsDenied_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeDeniedIdentity();

                var response = requirement.Creater.Add(item3, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsInvalid_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                requirement.Creater.Add(item3, identity);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsInvalid_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                var response = requirement.Creater.Add(item3, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode);
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsNull_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeValidItem();

                requirement.Creater.Add(item3, null);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsNull_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.AddItem();
                var item2 = requirement.Helper.AddItem();
                var state = requirement.Helper.GetAllItems();
                var item3 = requirement.Helper.MakeValidItem();

                var response = requirement.Creater.Add(item3, null);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsAllowed_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();
                
                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeAllowedIdentity();

                requirement.Creater.Add(item, identity);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsAllowed_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeAllowedIdentity();

                var response = requirement.Creater.Add(item, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsDenied_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeDeniedIdentity();

                requirement.Creater.Add(item, identity);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsDenied_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeDeniedIdentity();

                var response = requirement.Creater.Add(item, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsInvalid_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                requirement.Creater.Add(item, identity);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsInvalid_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                var response = requirement.Creater.Add(item, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsNull_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();

                requirement.Creater.Add(item, null);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsNull_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();

                var response = requirement.Creater.Add(item, null);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsNull_IdentityIsAllowed_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();
                
                var identity = requirement.Helper.MakeAllowedIdentity();

                requirement.Creater.Add(null, identity);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsNull_IdentityIsAllowed_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();
                
                var identity = requirement.Helper.MakeAllowedIdentity();

                var response = requirement.Creater.Add(null, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsNull_IdentityIsDenied_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var identity = requirement.Helper.MakeAllowedIdentity();

                var response = requirement.Creater.Add(null, identity);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsNull_IdentityIsDenied_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var identity = requirement.Helper.MakeDeniedIdentity();

                var response = requirement.Creater.Add(null, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsNull_IdentityIsInvalid_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var identity = requirement.Helper.MakeInvalidIdentity();

                var response = requirement.Creater.Add(null, identity);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsNull_IdentityIsInvalid_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var identity = requirement.Helper.MakeInvalidIdentity();

                var response = requirement.Creater.Add(null, identity);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsNull_IdentityIsNull_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var response = requirement.Creater.Add(null, null);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsNull_IdentityIsNull_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var response = requirement.Creater.Add(null, null);

                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsAllowed_IsAdded()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeAllowedIdentity();

                var response = requirement.Creater.Add(item, identity);
                var items = requirement.Helper.GetAllItems();

                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Item);
                Assert.That(response.Ok);
                Assert.That(items.Count == 1);
                Assert.That(requirement.Helper.AreEqual(items.First(), response.Item));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsAllowed_ReportsOk()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeAllowedIdentity();

                var response = requirement.Creater.Add(item, identity);
                                
                Assert.That(response.Ok);
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsDenied_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeDeniedIdentity();

                requirement.Creater.Add(item, identity);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsDenied_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeDeniedIdentity();

                var response = requirement.Creater.Add(item, identity);

                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsInvalid_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                requirement.Creater.Add(item, identity);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsInvalid_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                var response = requirement.Creater.Add(item, identity);

                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsNull_NoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();

                requirement.Creater.Add(item, null);

                Assert.That(requirement.Helper.ItemCountIs(0));
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsNull_ReportsError()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();

                var response = requirement.Creater.Add(item, null);

                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
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
