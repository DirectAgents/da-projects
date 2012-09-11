using System;
namespace CakeUtility
{
    public interface IExceptionLogger
    {
        int LogException(Exception e);
    }
}
