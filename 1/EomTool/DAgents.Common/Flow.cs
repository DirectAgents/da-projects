using System;

namespace DAgents.Common
{
    public static class Flow
    {
        public static void Sequence(params Action[] steps)
        {
            foreach (var step in steps)
                step();
        }
    }
}
