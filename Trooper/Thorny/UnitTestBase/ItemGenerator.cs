namespace Trooper.Thorny.UnitTestBase
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Thorny.Interface.UnitTestBase;

    public class ItemGenerator<TEnt, TPoco> : IItemGenerator<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        static ItemGenerator() 
        {
            AutoMapper.Mapper.CreateMap<TEnt, TEnt>();
        }

        public int ItemObjCount { get; set; }

        public virtual TEnt CopyItem(TEnt item)
        {
            var result = new TEnt();

            AutoMapper.Mapper.Map<TEnt, TEnt>(item, result);

            Assert.IsNotNull(result, "CopyItem returned null.");

            return result;
        }

        public virtual TEnt NewItem(IFacade<TEnt, TPoco> facade)
        {
            this.ItemObjCount++;
            var result = new TEnt();

            foreach (var p in result.GetType().GetProperties().Where(p => facade.KeyProperties.All(kp => kp.Name != p.Name)))
            {
                switch (Type.GetTypeCode(p.PropertyType))
                {
                    case TypeCode.String:
                        p.SetValue(result, string.Format("{0}_{1}", p.Name, ItemObjCount));
                        break;
                    case TypeCode.Boolean:
                        p.SetValue(result, false);
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

            Assert.IsNotNull(result, "NewItem returned null.");

            return result;
        }
    }
}
