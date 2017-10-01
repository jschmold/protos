using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    public static class LangHelpers
    {
        /// <summary>
        /// Do Action if it is not null, or throw the excepion e
        /// </summary>
        /// <param name="act">The action to perform</param>
        /// <param name="e">The exception to throw if act is null</param>
        public static void DoOrThrow(Action act, Exception e) {
            if (act == null) {
                throw e;
            } else {
                act.Invoke( );
            }
        }
    }
}
