namespace Trooper.Thorny.Business.Operation.Core
{
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public class CacheHit<TPoco> : ICacheHit<TPoco>
        where TPoco : class
    {
        public bool Hit { get; set; }

        public TPoco Item { get; set; }
    }
}
