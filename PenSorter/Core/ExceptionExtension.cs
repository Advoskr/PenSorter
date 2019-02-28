using System;

namespace PenSorter.Core
{
    public static class ExceptionExtension
    {
        public static string GetFullExceptionMessage(this Exception ex)
        {
            return ex.Message + ((ex.InnerException == null) ? "" : ex.InnerException.GetFullExceptionMessage());
        }
    }
}