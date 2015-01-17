namespace Trooper.BusinessOperation2.UnitTestBase
{
    using NUnit.Framework;
    using System;
    using Trooper.BusinessOperation2.Interface.UnitTestBase;

    public class ItemGenerator<Tc, Ti> : IItemGenerator<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        public int ItemObjCount { get; set; }

        public virtual Tc MakeItem(bool identical, Tc item)
        {
            if (identical)
            {
                return AutoMapper.Mapper.Map<Tc>(item);
            }

            var result = new Tc();

            foreach (var p in result.GetType().GetProperties())
            {
                switch (Type.GetTypeCode(p.PropertyType))
                {
                    case TypeCode.String:
                        p.SetValue(result, string.Format("{0}_{1}", p.Name, ItemObjCount));
                        break;
                    case TypeCode.Boolean:
                        p.SetValue(result, !((bool)p.GetValue(item)));
                        break;
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        p.SetValue(result, ItemObjCount);
                        break;
                }
            }

            return result;
        }

        public Tc ItemFactory()
        {
            return this.ItemFactory(false, new Tc()) as Tc;
        }

        public Tc ItemFactory(bool identical, Tc item)
        {
            if (!identical)
            {
                this.ItemObjCount++;
            }

            var result = this.MakeItem(identical, item);

            Assert.IsNotNull(result, "TestEntityFactory returned null.");

            return result;
        }
    }
}
