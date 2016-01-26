namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System.Linq;
    using Response;
    using System;

    public sealed class GetAllDataStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));
            if (!(stepInfo.response is ManyResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(ManyResponse<TEnt>)}");

            var manyResponse = stepInfo.response as ManyResponse<TEnt>;
            manyResponse.Items = stepInfo.businessPack.Facade.GetAll().ToList();
        }
    }
}
