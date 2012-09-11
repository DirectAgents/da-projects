using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectAgents.Common
{
    public interface IFactory<out T>
    {
        T Create();
    }
}
