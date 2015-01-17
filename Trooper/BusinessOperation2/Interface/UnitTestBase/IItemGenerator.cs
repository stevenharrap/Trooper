using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.BusinessOperation2.Interface.UnitTestBase
{
    public interface IItemGenerator<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        int ItemObjCount { get; set; }

        Tc MakeItem(bool identical, Tc entity);

        Tc ItemFactory();

        Tc ItemFactory(bool identical, Tc item);
    }
}
