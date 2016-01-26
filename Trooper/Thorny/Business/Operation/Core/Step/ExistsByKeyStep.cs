namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System;
    using Response;

    public sealed class ExistsByKeyStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.item == null) throw new ArgumentNullException(nameof(stepInfo.item));
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));
            if (!(stepInfo.response is SingleResponse<bool>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(SingleResponse<bool>)}");

            var boolResponse = stepInfo.response as SingleResponse<bool>;

            var result = stepInfo.businessPack.Facade.GetByKey(stepInfo.item);
            boolResponse.Item = result != null;
        }
    }
}
