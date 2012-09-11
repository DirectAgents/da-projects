using System;
using System.Collections.Generic;
using Cake.Model;
using Microsoft.Practices.Unity;

namespace CakeUtility
{
    public class DefaultExceptionLogger : IExceptionLogger
    {
        public int LogException(Exception e)
        {
            using (var db = new CakeEntities())
            {
                var error = CakeUtilityError(e);
                db.CakeUtilityErrors.AddObject(error);
                db.SaveChanges();
                this.CakeUtilityErrorIds.Add(error.Id);
                return error.Id;
            }
        }

        private static CakeUtilityError CakeUtilityError(Exception e)
        {
            var result = new CakeUtilityError();

            result.Tag = Tag(e);
            result.ExceptionMessage = ExceptionMessage(e);
            result.ExceptionStackTrace = e.StackTrace;

            if (e.InnerException != null)
            {
                result.InnerExceptionMessage = e.InnerException.Message;
                result.InnerExceptionMessage = e.InnerException.StackTrace;
            }

            return result;
        }

        private static string ExceptionMessage(Exception e)
        {
            string result = string.Empty;
            Exception exception = e;
            while (exception != null)
            {
                if (e.Message != null)
                {
                    if (result.Length > 0)
                    {
                        result += Separator;
                    }
                    result += exception.Message;
                }
                exception = exception.InnerException;
            }
            return result;
        }

        private static string Tag(Exception e)
        {
            string result = string.Empty;
            Exception exception = e;
            while (exception != null)
            {
                if (e.TargetSite != null)
                {
                    if (result.Length > 0)
                    {
                        result += Separator;
                    }
                    result += exception.TargetSite.Name;
                }
                exception = exception.InnerException;
            }
            return result;
        }
         
        private static string Separator { get { return " ---> "; } } 
        
        [Dependency("CakeUtilityErrorIds")]
        public ICollection<int> CakeUtilityErrorIds { get; set; }
    }
}
