namespace Trooper.Thorny.Utility
{
    using Autofac;
    using System;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Interface.DataManager;

    public class BusinessUtility<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public static TIBusinessPack GetBusinessPack<TIBusinessPack>(IContainer container, IUnitOfWork uow = null)
            where TIBusinessPack : class, IBusinessPack<TEnt, TPoco>
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            var bp = container.Resolve<IBusinessCore<TEnt, TPoco>>();

            return bp.GetBusinessPack(uow) as TIBusinessPack;
        }

        public static IBusinessPack<TEnt, TPoco> GetBusinessPack(IContainer container, IUnitOfWork uow = null)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            var bp = container.Resolve<IBusinessCore<TEnt, TPoco>>();

            return bp.GetBusinessPack(uow);
        }        
    }

    public class BusinessUtility
    {
        public static IUnitOfWork GetUow(IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            return container.Resolve<IUnitOfWork>();
        }
    }
}