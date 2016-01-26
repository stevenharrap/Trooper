namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System.Linq;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System;
    using Response;

    public sealed class GetSomeStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.search == null) throw new ArgumentNullException(nameof(stepInfo.search));
            if (!(stepInfo.response is ManyResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(ManyResponse<TEnt>)}");

            var manyResponse = stepInfo.response as ManyResponse<TEnt>;
            var some = stepInfo.businessPack.Facade.GetSome(stepInfo.search);
            var limit = stepInfo.search.SkipItems > 0 && stepInfo.search.TakeItems > 0;

            if (limit)
            {
                manyResponse.Items = stepInfo.businessPack.Facade.Limit(some, stepInfo.search).ToList();
            }
            else
            {
                manyResponse.Items = some.ToList();
            }
        }
    }
}
