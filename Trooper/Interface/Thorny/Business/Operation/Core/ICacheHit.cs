namespace Trooper.Interface.Thorny.Business.Operation.Core
{
    public interface ICacheHit<TPoco>        
        where TPoco : class
    {
        bool Hit { get; set; }

        TPoco Item { get; set; }
    }
}