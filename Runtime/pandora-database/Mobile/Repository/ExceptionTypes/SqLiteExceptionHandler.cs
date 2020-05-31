

using System;

namespace Unity.Pandora.Database.Mobile.Repository.ExceptionTypes
{
    public class SqLiteExceptionHandler<TObject> where TObject : Exception
    {

        private TObject error;

        public SqLiteExceptionHandler(TObject error)
        {
            this.error = error;
        }

        public string GetDescription()
        {
            string desc = error.Message;
            return desc;
        }

        public string GetErrorType()
        {
            string typeEx = error.GetType().ToString();
            return typeEx;
        }

        public string GetTrackTrace()
        {
            string typeEx = error.StackTrace;
            return typeEx;
        }

    }
}
