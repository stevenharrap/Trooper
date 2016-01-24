
namespace Trooper.Thorny.Business.TestSuit.Self
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Operation.Core;
    using System.Collections.Generic;
    using Utility;
    using Trooper.Interface.Thorny.Business.Response;

    public abstract class TestSelf<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<SelfRequirement<TPoco>> Requirement { get; }
        
        [Test]
        public void DefaultRequiredValidItems()
        {
            using (var requirement = this.Requirement())
            {
                Assert.That(requirement.Helper.DefaultRequiredValidItems, Is.GreaterThan(1));
            }
        }

        [Test]
        public void DefaultRequiredInvalidItems()
        {
            using (var requirement = this.Requirement())
            {
                Assert.That(requirement.Helper.DefaultRequiredInvalidItems, Is.GreaterThan(1));
            }
        }

        [Test]
        public void DefaultRequiredItems()
        {
            using (var requirement = this.Requirement())
            {
                var a = requirement.Helper.DefaultRequiredValidItems;
                var b = requirement.Helper.DefaultRequiredInvalidItems;
                var greater = a > b ? a : b;

                Assert.That(requirement.Helper.DefaultRequiredItems, Is.EqualTo(greater));
            }
        }

        [Test]
        public void MakeValidItems()
        {
            using (var requirement = this.Requirement())
            {
                var defaultItem = new TPoco();

                var result = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredValidItems));
                Assert.That(result.Any(i => i == null), Is.False);
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => !requirement.Helper.IdentifiersAreEqual(defaultItem, i)), Is.True);
                var result1 = result.Select((v, i) => new { v, i });
                var result2 = result.Select((v, i) => new { v, i });
                Assert.That(result1.All(r1 => result2.Where(r2 => r1.i != r2.i).All(r2 => !requirement.Helper.IdentifiersAreEqual(r1.v, r2.v))), Is.True);

                result = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Default);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredValidItems));
                Assert.That(result.Any(i => i == null), Is.False);
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => requirement.Helper.IdentifiersAreEqual(defaultItem, i)), Is.True);

                var twice = requirement.Helper.DefaultRequiredValidItems * 2;

                result = requirement.Helper.MakeValidItems(twice, TestSuitHelper.Keys.Gen);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(twice));
                Assert.That(result.Any(i => i == null), Is.False);
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => !requirement.Helper.IdentifiersAreEqual(defaultItem, i)), Is.True);
                result1 = result.Select((v, i) => new { v, i });
                result2 = result.Select((v, i) => new { v, i });
                Assert.That(result1.All(r1 => result2.Where(r2 => r1.i != r2.i).All(r2 => !requirement.Helper.IdentifiersAreEqual(r1.v, r2.v))), Is.True);

                result = requirement.Helper.MakeValidItems(twice, TestSuitHelper.Keys.GenIfMnl);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(twice));
                Assert.That(result.Any(i => i == null), Is.False);
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => requirement.Helper.IdentifiersAreEqual(defaultItem, i)), Is.True);
                //Assert.That(result.All(r1 => result.Where(r2 => !ReferenceEquals(r1, r2)).All(r2 => !this.IdentifiersAreEqual(r1, r2))), Is.True);

                result = requirement.Helper.MakeValidItems(twice, TestSuitHelper.Keys.Default);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(twice));
                Assert.That(result.Any(i => i == null), Is.False);
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => requirement.Helper.IdentifiersAreEqual(defaultItem, i)), Is.True);

                var otherItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen);

                result = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, otherItems);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredValidItems));
                Assert.That(result.Any(i => i == null), Is.False);
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => !requirement.Helper.IdentifiersAreEqual(defaultItem, i)), Is.True);
                Assert.That(result.All(i => !otherItems.Any(oi => requirement.Helper.IdentifiersAreEqual(i, oi))), Is.True);
                result1 = result.Select((v, i) => new { v, i });
                result2 = result.Select((v, i) => new { v, i });
                Assert.That(result1.All(r1 => result2.Where(r2 => r1.i != r2.i).All(r2 => !requirement.Helper.IdentifiersAreEqual(r1.v, r2.v))), Is.True);

                result = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Default, otherItems);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredValidItems));
                Assert.That(result.Any(i => i == null), Is.False);
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => requirement.Helper.IdentifiersAreEqual(defaultItem, i)), Is.True);
            }
        }

        [Test]
        public void MakeInvalidItems()
        {
            using (var requirement = this.Requirement())
            {
                var items = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                var original = requirement.Helper.Copy(items);
                requirement.Helper.MakeInvalidItems(items);
                Assert.That(items.Count(), Is.EqualTo(original.Count()));
                Assert.That(items.Count(i => i == null), Is.EqualTo(0));
                Assert.That(HasDuplicateReference(items), Is.False);
                for (var i = 0; i < original.Count(); i++)
                {
                    Assert.That(requirement.Helper.IdentifiersAreEqual(original.ElementAt(i), items[i]), Is.True);
                }
                Assert.That(items.All(i => original.Any(o => !requirement.Helper.NonIdentifiersAreEqual(i, o))), Is.True);

                items = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                var otherItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, items);
                original = requirement.Helper.Copy(items);
                requirement.Helper.MakeInvalidItems(items, otherItems);
                Assert.That(items.Count(), Is.EqualTo(original.Count()));
                Assert.That(items.Count(i => i == null), Is.EqualTo(0));
                Assert.That(HasDuplicateReference(items), Is.False);
                for (var i = 0; i < original.Count(); i++)
                {
                    Assert.That(requirement.Helper.IdentifiersAreEqual(original.ElementAt(i), items[i]), Is.True);
                }
                Assert.That(items.All(i => otherItems.Any(o => !requirement.Helper.NonIdentifiersAreEqual(i, o))), Is.True);

                items = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                otherItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, items);
                original = requirement.Helper.Copy(items);
                requirement.Helper.MakeInvalidItems(true, items, otherItems);
                Assert.That(items.Count(), Is.EqualTo(original.Count() + 1));
                Assert.That(items.Count(i => i == null), Is.EqualTo(1));
                Assert.That(HasDuplicateReference(items), Is.False);
                for (var i = 0; i < original.Count(); i++)
                {
                    if (items[i] == null) continue;

                    Assert.That(requirement.Helper.IdentifiersAreEqual(original.ElementAt(i), items[i]), Is.True);
                }
                Assert.That(items.All(i => i == null || otherItems.Any(o => !requirement.Helper.NonIdentifiersAreEqual(i, o))), Is.True);

                var result = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.Gen);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredInvalidItems));
                Assert.That(HasDuplicateReference(result), Is.False);

                result = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.Gen);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredInvalidItems));
                Assert.That(HasDuplicateReference(result), Is.False);

                result = requirement.Helper.MakeInvalidItems(false, requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.Gen);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredInvalidItems * 2));
                Assert.That(HasDuplicateReference(result), Is.False);

                result = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.Gen);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredInvalidItems + 1));
                Assert.That(HasDuplicateReference(result), Is.False);

                result = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.Gen, otherItems);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredInvalidItems));
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => otherItems.Any(o => !requirement.Helper.NonIdentifiersAreEqual(i, o))), Is.True);

                result = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.Gen, otherItems);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredInvalidItems));
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => otherItems.Any(o => !requirement.Helper.NonIdentifiersAreEqual(i, o))), Is.True);

                result = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.Gen, otherItems);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredInvalidItems + 1));
                Assert.That(HasDuplicateReference(result), Is.False);
                Assert.That(result.All(i => i == null || otherItems.Any(o => !requirement.Helper.NonIdentifiersAreEqual(i, o))), Is.True);
                Assert.That(result.Count(i => i == null), Is.EqualTo(1));
            }
        }

        [Test]
        public void MakeInvalidItem()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                {
                    var item = requirement.Helper.Copy(validItem);
                    requirement.Helper.MakeInvalidItem(item);

                    Assert.That(validItem, Is.Not.Null);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(validItem, item), Is.False);
                }
            }
        }

        [Test]
        public void AreEqual()
        {
            using (var requirement = this.Requirement())
            {
                var itemsA = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen);
                var itemsB = requirement.Helper.Copy(itemsA);

                for (var i = 0; i < itemsA.Count(); i++)
                {
                    var itemA = itemsA.ElementAt(i);
                    var copyA = requirement.Helper.Copy(itemA);
                    var itemB = itemsA.ElementAt(i);
                    var copyB = requirement.Helper.Copy(itemB);

                    Assert.That(requirement.Helper.AreEqual(itemA, copyA), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemB, copyB), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemA, itemB), Is.True);
                    Assert.That(requirement.Helper.IdentifiersAreEqual(itemA, itemB), Is.True);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(itemA, itemB), Is.True);
                }

                Assert.That(requirement.Helper.AreEqual(itemsA, itemsB), Is.True);
            }
        }

        [Test]
        public void Contains()
        {
            using (var requirement = this.Requirement())
            {
                var items1 = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen);
                var items2 = requirement.Helper.Copy(items1);
                var empty = new List<TPoco>();
                var partial = new List<TPoco> { items1.First() };
                var different = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, items1);

                Assert.That(requirement.Helper.Contains(items1, items2), Is.True);
                Assert.That(requirement.Helper.Contains(items2, items1), Is.True);
                Assert.That(requirement.Helper.Contains(items1, partial), Is.True);
                Assert.That(requirement.Helper.Contains(partial, items1), Is.False);
                Assert.That(requirement.Helper.Contains(items1, empty), Is.True);
                Assert.That(requirement.Helper.Contains(empty, items1), Is.False);
                Assert.That(requirement.Helper.Contains(empty, empty), Is.True);
                Assert.That(requirement.Helper.Contains(different, items1), Is.False);
            }
        }

        [Test]
        public void ContainsIdentifiers()
        {
            using (var requirement = this.Requirement())
            {
                var items1 = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen);
                var items2 = requirement.Helper.Copy(items1);
                requirement.Helper.ChangeNonIdentifiers(items1);
                var empty = new List<TPoco>();
                var partial = new List<TPoco> { items1.First() };
                var different = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, items1);

                Assert.That(requirement.Helper.ContainsIdentifiers(items1, items2), Is.True);
                Assert.That(requirement.Helper.ContainsIdentifiers(items2, items1), Is.True);
                Assert.That(requirement.Helper.ContainsIdentifiers(items1, partial), Is.True);
                Assert.That(requirement.Helper.ContainsIdentifiers(partial, items1), Is.False);
                Assert.That(requirement.Helper.ContainsIdentifiers(items1, empty), Is.True);
                Assert.That(requirement.Helper.ContainsIdentifiers(empty, items1), Is.False);
                Assert.That(requirement.Helper.ContainsIdentifiers(empty, empty), Is.True);
                Assert.That(requirement.Helper.ContainsIdentifiers(different, items1), Is.False);
            }
        }

        [Test]
        public void ContainsNonIdentifiers()
        {
            using (var requirement = this.Requirement())
            {
                var items1 = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                var items2 = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, items1).ToList();
                requirement.Helper.CopyNonIdentifiers(items1, items2);
                var empty = new List<TPoco>();
                var partial = new List<TPoco> { items1.First() };
                var different = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, items1);

                Assert.That(requirement.Helper.ContainsNonIdentifiers(items1, items2), Is.True);
                Assert.That(requirement.Helper.ContainsNonIdentifiers(items2, items1), Is.True);
                Assert.That(requirement.Helper.ContainsNonIdentifiers(items1, partial), Is.True);
                Assert.That(requirement.Helper.ContainsNonIdentifiers(partial, items1), Is.False);
                Assert.That(requirement.Helper.ContainsNonIdentifiers(items1, empty), Is.True);
                Assert.That(requirement.Helper.ContainsNonIdentifiers(empty, items1), Is.False);
                Assert.That(requirement.Helper.ContainsNonIdentifiers(empty, empty), Is.True);
                Assert.That(requirement.Helper.ContainsNonIdentifiers(different, items1), Is.False);
            }
        }

        [Test]
        public void IdentifiersAreEqual()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var itemA in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                {
                    var originalItemA = requirement.Helper.Copy(itemA);
                    var itemB = requirement.Helper.Copy(itemA);
                    var originalItemB = requirement.Helper.Copy(itemB);

                    Assert.That(requirement.Helper.IdentifiersAreEqual(itemA, itemB), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemA, originalItemA), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemB, originalItemB), Is.True);

                    var itemsC = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                    var originalItemsC = requirement.Helper.Copy(itemsC);
                    var itemsD = requirement.Helper.Copy(itemsC);
                    var originalItemsD = requirement.Helper.Copy(itemsD);

                    Assert.That(requirement.Helper.IdentifiersAreEqual(itemsC, itemsD), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemsC, originalItemsC), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemsD, originalItemsD), Is.True);
                }
            }
        }

        [Test]
        public void NonIdentifiersAreEqual()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var itemA in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                {
                    var originalItemA = requirement.Helper.Copy(itemA);
                    var itemB = requirement.Helper.Copy(itemA);
                    var originalItemB = requirement.Helper.Copy(itemB);

                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(itemA, itemB), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemA, originalItemA), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemB, originalItemB), Is.True);

                    var itemsC = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                    var originalItemsC = requirement.Helper.Copy(itemsC);
                    var itemsD = requirement.Helper.Copy(itemsC);
                    var originalItemsD = requirement.Helper.Copy(itemsD);

                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(itemsC, itemsD), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemsC, originalItemsC), Is.True);
                    Assert.That(requirement.Helper.AreEqual(itemsD, originalItemsD), Is.True);
                }
            }
        }

        [Test]
        public void Copy()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                {
                    var copiedItem = requirement.Helper.Copy(validItem);

                    Assert.That(ReferenceEquals(copiedItem, validItem), Is.False);
                    Assert.That(requirement.Helper.AreEqual(validItem, copiedItem));
                }

                var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen);
                var orginalCount = validItems.Count();
                var copiedItems = requirement.Helper.Copy(validItems);

                Assert.That(copiedItems, Is.Not.Null);
                Assert.That(copiedItems.Count, Is.EqualTo(orginalCount));
                Assert.That(requirement.Helper.AreEqual(validItems, copiedItems), Is.True);
                Assert.That(HasDuplicateReference(validItems, copiedItems), Is.False);

                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                {
                    var copiedItem = new TPoco();
                    requirement.Helper.Copy(validItem, copiedItem);

                    Assert.That(requirement.Helper.AreEqual(validItem, copiedItem), Is.True);
                }
            }
        }

        [Test]
        public void CopyIdentifiers()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                {
                    var defaultItem = new TPoco();
                    var copiedItem = requirement.Helper.CopyIdentifiers(validItem);

                    Assert.That(copiedItem, Is.Not.Null);
                    Assert.That(ReferenceEquals(copiedItem, validItem), Is.False);
                    Assert.That(requirement.Helper.IdentifiersAreEqual(copiedItem, validItem), Is.True);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(copiedItem, defaultItem), Is.True);
                }

                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                {
                    var defaultItem = new TPoco();
                    requirement.Helper.ChangeNonIdentifiers(defaultItem, validItem);
                    requirement.Helper.CopyIdentifiers(validItem, defaultItem);

                    Assert.That(requirement.Helper.IdentifiersAreEqual(validItem, defaultItem), Is.True);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(validItem, defaultItem), Is.False);
                }
            }
        }

        [Test]
        public void CopyNonIdentifiers()
        {
            using (var requirement = this.Requirement())
            {
                var defaultItem = new TPoco();

                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                {
                    var copiedItem = requirement.Helper.CopyNonIdentifiers(validItem);

                    Assert.That(copiedItem, Is.Not.Null);
                    Assert.That(ReferenceEquals(copiedItem, validItem), Is.False);
                    Assert.That(requirement.Helper.IdentifiersAreEqual(copiedItem, defaultItem), Is.True);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(copiedItem, validItem), Is.True);
                }

                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                {
                    requirement.Helper.ChangeNonIdentifiers(defaultItem, validItem);
                    requirement.Helper.CopyNonIdentifiers(validItem, defaultItem);

                    Assert.That(requirement.Helper.IdentifiersAreEqual(validItem, defaultItem), Is.False);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(validItem, defaultItem), Is.True);
                }

                var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                var destination = new List<TPoco>();

                for (var i = 0; i < validItems.Count(); i++)
                {
                    destination.Add(new TPoco());
                }

                requirement.Helper.CopyNonIdentifiers(validItems, destination);

                Assert.That(HasDuplicateReference(validItems, destination), Is.False);

                for (var i = 0; i < validItems.Count(); i++)
                {
                    Assert.That(requirement.Helper.IdentifiersAreEqual(validItems[i], destination[i]), Is.False);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(validItems[i], destination[i]), Is.True);
                }
            }
        }

        [Test]
        public void ChangeNonIdentifiers()
        {
            using (var requirement = this.Requirement())
            {
                var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();

                for (var t = 0; t < validItems.Count(); t++)
                {
                    var others = new List<TPoco>();
                    var target = validItems[t];
                    var original = requirement.Helper.Copy(validItems[t]);

                    for (var i = 0; i < validItems.Count(); i++)
                    {
                        if (i != t)
                        {
                            others.Add(validItems[i]);
                        }
                    }

                    requirement.Helper.ChangeNonIdentifiers(target, others);

                    Assert.That(requirement.Helper.IdentifiersAreEqual(target, original), $"The {nameof(requirement.Helper.ChangeNonIdentifiers)} method should not change the identifier properties");
                    Assert.That(!requirement.Helper.NonIdentifiersAreEqual(target, original), $"The {nameof(requirement.Helper.ChangeNonIdentifiers)} method should change the none-identifer properties of the item to different values.");

                    foreach (var item in others)
                    {
                        Assert.That(requirement.Helper.AreEqual(target, item), Is.Not.True, "The target item should not be Equal to any other item");
                    }
                }

                var list1 = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                var list2 = requirement.Helper.Copy(list1).ToList();

                for (var i = 0; i < list1.Count(); i++)
                {
                    var i1 = list1[i];
                    var i2 = list2[i];
                    requirement.Helper.ChangeNonIdentifiers(i1, i2);

                    Assert.That(requirement.Helper.IdentifiersAreEqual(i1, i2), Is.True);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(i1, i2), Is.False);
                }

                list1 = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                list2 = requirement.Helper.Copy(list1).ToList();
                requirement.Helper.ChangeNonIdentifiers(list1);

                for (var i = 0; i < list1.Count(); i++)
                {
                    var i1 = list1[i];
                    var i2 = list2[i];

                    Assert.That(requirement.Helper.IdentifiersAreEqual(i1, i2), Is.True);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(i1, i2), Is.False);
                }

                list1 = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen).ToList();
                list2 = requirement.Helper.Copy(list1).ToList();
                requirement.Helper.ChangeNonIdentifiers(list1, list2);
                Assert.That(list1.All(i1 => !list2.Any(i2 => requirement.Helper.NonIdentifiersAreEqual(i1, i2))), Is.True);

                for (var i = 0; i < list1.Count(); i++)
                {
                    var i1 = list1[i];
                    var i2 = list2[i];

                    Assert.That(requirement.Helper.IdentifiersAreEqual(i1, i2), Is.True);
                }
            }
        }

        [Test]
        public void ChangeProperty()
        {
            using (var requirement = this.Requirement())
            {
                var pattern = "thing {0} x";
                var current = "thing 1 x";
                var otherValues = new List<string> { "thing 2 x", "thing 3 x" };
                var result = requirement.Helper.ChangeProperty(current, otherValues, pattern);
                Assert.That(result, Is.EqualTo("thing 4 x"));

                otherValues = new List<string> { "thing 3 x", "thing 4 x" };
                result = requirement.Helper.ChangeProperty(current, otherValues, pattern);
                Assert.That(result, Is.EqualTo("thing 2 x"));

                result = requirement.Helper.ChangeProperty(otherValues, pattern);
                Assert.That(result, Is.EqualTo("thing 1 x"));

                otherValues = new List<string> { "thing 1 x", "thing 2 x" };
                result = requirement.Helper.ChangeProperty(otherValues, pattern);
                Assert.That(result, Is.EqualTo("thing 3 x"));

                var current2 = 5;
                var otherValues2 = new List<int> { 4, 7, 3 };
                var result2 = requirement.Helper.ChangeProperty(current2, otherValues2);
                Assert.That(result2, Is.EqualTo(6));

                current2 = 7;
                result2 = requirement.Helper.ChangeProperty(current2, otherValues2);
                Assert.That(result2, Is.EqualTo(8));

                result2 = requirement.Helper.ChangeProperty(otherValues2);
                Assert.That(result2, Is.EqualTo(1));
            }
        }

        [Test]
        public void MakeKeyValue()
        {
            using (var requirement = this.Requirement())
            {
                var others = new List<int?> { 1, 4, 7, 3 };

                var result = requirement.Helper.MakeKeyValue(TestSuitHelper.Keys.Default, false, others);
                Assert.That(result, Is.EqualTo(0));

                result = requirement.Helper.MakeKeyValue(TestSuitHelper.Keys.Default, true, others);
                Assert.That(result, Is.EqualTo(0));

                result = requirement.Helper.MakeKeyValue(TestSuitHelper.Keys.Gen, false, others);
                Assert.That(result, Is.EqualTo(2));

                result = requirement.Helper.MakeKeyValue(TestSuitHelper.Keys.Gen, true, others);
                Assert.That(result, Is.EqualTo(2));

                result = requirement.Helper.MakeKeyValue(TestSuitHelper.Keys.GenIfMnl, false, others);
                Assert.That(result, Is.EqualTo(2));

                result = requirement.Helper.MakeKeyValue(TestSuitHelper.Keys.GenIfMnl, true, others);
                Assert.That(result, Is.EqualTo(0));
            }
        }

        [Test]
        public void MakeAllowedIdentities()
        {
            using (var requirement = this.Requirement())
            {
                var allowedIdentities = requirement.Helper.MakeAllowedIdentities().ToList();

                Assert.That(allowedIdentities, Is.Not.Null);
                Assert.That(allowedIdentities.Count(), Is.GreaterThan(0));
                Assert.That(allowedIdentities.Count(i => i == null), Is.EqualTo(0));
                Assert.That(HasDuplicateReference(allowedIdentities), Is.False);

                foreach (var identity in allowedIdentities)
                {
                    requirement.Helper.RemoveAllItems();
                    var validItem = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl).First();
                    var result = requirement.Helper.BoCreater.Add(validItem, identity);

                    Assert.That(requirement.Helper.ResponseIsOk(result), Is.True);
                }
            }
        }

        [Test]
        public void MakeDeniedIdentities()
        {
            using (var requirement = this.Requirement())
            {
                var deniedIdentities = requirement.Helper.MakeDeniedIdentities().ToList();

                Assert.That(deniedIdentities, Is.Not.Null);
                Assert.That(deniedIdentities.Count(), Is.GreaterThan(0));
                Assert.That(deniedIdentities.Count(i => i == null), Is.EqualTo(0));
                Assert.That(HasDuplicateReference(deniedIdentities), Is.False);

                foreach (var identity in deniedIdentities)
                {
                    requirement.Helper.RemoveAllItems();
                    var validItem = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl).First();
                    var result = requirement.Helper.BoCreater.Add(validItem, identity);

                    Assert.That(requirement.Helper.ResponseFailsWithError(result, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public void MakeInvalidIdentities()
        {
            using (var requirement = this.Requirement())
            {
                var invalidIdentities = requirement.Helper.MakeInvalidIdentities().ToList();

                Assert.That(invalidIdentities, Is.Not.Null);
                Assert.That(invalidIdentities.Count(), Is.GreaterThan(1));
                Assert.That(invalidIdentities.Count(i => i == null), Is.EqualTo(1));
                Assert.That(HasDuplicateReference(invalidIdentities), Is.False);

                foreach (var identity in invalidIdentities)
                {
                    requirement.Helper.RemoveAllItems();
                    var validItem = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl).First();
                    var result = requirement.Helper.BoCreater.Add(validItem, identity);

                    Assert.That(requirement.Helper.ResponseFailsWithError(result, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public void GetAdminIdentity()
        {
            using (var requirement = this.Requirement())
            {
                var identity = requirement.Helper.GetAdminIdentity();

                Assert.That(identity, Is.Not.Null);

                requirement.Helper.RemoveAllItems();
                var validItem = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl).First();
                var response = requirement.Helper.BoCreater.Add(validItem, identity) as IResponse;
                Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);

                requirement.Helper.RemoveAllItems();
                var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                response = requirement.Helper.BoCreater.AddSome(validItems, identity);
                Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);

                requirement.Helper.RemoveAllItems();
                validItems = requirement.Helper.AddValidItems();
                response = requirement.Helper.BoDeleter.DeleteByKey(validItems.First(), identity);
                Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);

                requirement.Helper.RemoveAllItems();
                validItems = requirement.Helper.AddValidItems();
                response = requirement.Helper.BoDeleter.DeleteSomeByKey(validItems, identity);
                Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);

                requirement.Helper.RemoveAllItems();
                validItems = requirement.Helper.AddValidItems();

                response = requirement.Helper.BoReader.ExistsByKey(validItems.First(), identity);
                Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);

                response = requirement.Helper.BoReader.GetByKey(validItems.First(), identity);
                Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);

                response = requirement.Helper.BoReader.GetByKey(validItems.First(), identity);
                Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);

                //Todo: test that search by works for admin
            }
        }

        [Test]
        public void AddItems()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl).ToList();

                var items = requirement.Helper.AddItems(validItems);

                Assert.That(validItems.Count(), Is.EqualTo(items.Count()));
                Assert.That(validItems.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredValidItems));
            }
        }

        [Test]
        public void AddValidItems()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                {
                    var items = requirement.Helper.AddValidItems();

                    Assert.That(items, Is.Not.Null);
                    Assert.That(items.Count(), Is.GreaterThan(1));
                    Assert.That(items.Count(i => i == null), Is.EqualTo(0));
                    Assert.That(items.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredValidItems));
                }

                {
                    var required = requirement.Helper.DefaultRequiredValidItems * 2;
                    var items = requirement.Helper.AddValidItems(required);

                    Assert.That(items, Is.Not.Null);
                    Assert.That(items.Count(), Is.EqualTo(required));
                    Assert.That(items.Count(i => i == null), Is.EqualTo(0));
                }

                foreach (var item in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                {
                    requirement.Helper.RemoveAllItems();

                    var items = requirement.Helper.AddValidItems(item);

                    Assert.That(items, Is.Not.Null);
                    Assert.That(items.Count(), Is.GreaterThan(1));
                    Assert.That(items.Count(i => i == null), Is.EqualTo(0));
                    Assert.That(items.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredValidItems));
                    Assert.That(items.All(i => !requirement.Helper.AreEqual(i, item)), Is.True);
                }

                {
                    requirement.Helper.RemoveAllItems();

                    var otherItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var items = requirement.Helper.AddValidItems(otherItems);

                    Assert.That(items, Is.Not.Null);
                    Assert.That(items.Count(), Is.GreaterThan(1));
                    Assert.That(items.Count(i => i == null), Is.EqualTo(0));
                    Assert.That(items.Count(), Is.EqualTo(requirement.Helper.DefaultRequiredValidItems));
                    Assert.That(items.All(i => otherItems.All(o => !requirement.Helper.AreEqual(i, o))), Is.True);
                }
            }
        }

        [Test]
        public void AddItem()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var validItem = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl).First();

                var addedItem = requirement.Helper.AddItem(validItem);

                Assert.That(addedItem, Is.Not.Null);
            }
        }

        [Test]
        public void GetItem()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var addedItems = requirement.Helper.AddValidItems();

                var first = requirement.Helper.GetItem(addedItems.First());

                Assert.That(requirement.Helper.AreEqual(addedItems.First(), first), Is.True);
            }
        }

        [Test]
        public void ItemExists()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var addedItems = requirement.Helper.AddValidItems();

                var first = requirement.Helper.ItemExists(addedItems.First());

                Assert.That(first, Is.True);
            }
        }

        [Test]
        public void RemoveAllItems()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var allItems = requirement.Helper.GetAllItems();

                Assert.That(allItems.Count(), Is.EqualTo(0));

                requirement.Helper.AddValidItems();

                allItems = requirement.Helper.GetAllItems();

                Assert.That(allItems.Count(), Is.GreaterThan(0));

                requirement.Helper.RemoveAllItems();

                allItems = requirement.Helper.GetAllItems();

                Assert.That(allItems.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public void GetAllItems()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var addedItems = requirement.Helper.AddValidItems();

                var allItems = requirement.Helper.GetAllItems();

                Assert.That(allItems.Count(), Is.EqualTo(addedItems.Count()));
                Assert.That(HasDuplicateReference(allItems), Is.False);
            }
        }

        [Test]
        public void ItemCountIs()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                requirement.Helper.AddValidItems();

                var count = requirement.Helper.GetAllItems().Count();

                Assert.That(requirement.Helper.ItemCountIs(count), Is.True);
                Assert.That(requirement.Helper.ItemCountIs(count + 1), Is.False);
            }
        }

        [Test]
        public void HasNoItems()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                Assert.That(requirement.Helper.HasNoItems(), Is.True);

                requirement.Helper.AddValidItems();

                Assert.That(requirement.Helper.HasNoItems(), Is.False);
            }
        }

        [Test]
        public void StoredItemsAreEqualTo()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var items1 = requirement.Helper.AddValidItems();
                var items2 = requirement.Helper.Copy(items1).ToList();
                var items3 = requirement.Helper.Copy(items1).ToList();

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(items2), Is.True);
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(new List<TPoco>()), Is.False);

                items2.RemoveAt(items2.Count() - 1);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(items2), Is.False);

                requirement.Helper.ChangeNonIdentifiers(items3);

                Assert.That(requirement.Helper.StoredItemsAreEqualTo(items3), Is.False);
            }
        }

        [Test]
        public void ResponseIsOk()
        {
            using (var requirement = this.Requirement())
            {
                var response1 = new Response.Response();

                MessageUtility.Errors.Add("A message", "A code", response1);

                Assert.That(requirement.Helper.ResponseIsOk(response1), Is.False);

                var response2 = new Response.Response();

                Assert.That(requirement.Helper.ResponseIsOk(response2), Is.True);
            }
        }

        [Test]
        public void ResponseFailsWithError()
        {
            using (var requirement = this.Requirement())
            {
                var response1 = new Response.Response();

                MessageUtility.Errors.Add("A message", "A code", response1);

                Assert.That(requirement.Helper.ResponseFailsWithError(response1, "A code"), Is.True);
                Assert.That(requirement.Helper.ResponseFailsWithError(response1, "Another code"), Is.False);

                var response2 = new Response.Response();

                Assert.That(requirement.Helper.ResponseFailsWithError(response2, "A code"), Is.False);
            }
        }

        private bool HasDuplicateReference(IEnumerable<object> list)
        {
            using (var requirement = this.Requirement())
            {
                if (list == null) throw new ArgumentNullException(nameof(list));

                for (var a = 0; a < list.Count(); a++)
                {
                    for (var b = 0; b < list.Count(); b++)
                    {
                        if (a == b) continue;

                        if (ReferenceEquals(list.ElementAt(a), list.ElementAt(b)))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private bool HasDuplicateReference(IEnumerable<TPoco> list1, IEnumerable<TPoco> list2)
        {
            using (var requirement = this.Requirement())
            {
                if (list1.Any(i1 => list2.Any(i2 => ReferenceEquals(i1, i2))))
                {
                    return true;
                }

                if (list2.Any(i2 => list1.Any(i1 => ReferenceEquals(i1, i2))))
                {
                    return true;
                }

                return false;
            }
        }        
    }
}