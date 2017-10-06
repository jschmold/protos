using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities {
    
    public static class LangHelpers {
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

        public static bool ContainsAll(this List<Object> obj, List<Object> all) {
            foreach (var val in all) {
                if (!obj.Contains(val)) {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Call success if test is true, otherwise call fail
        /// </summary>
        /// <param name="test"></param>
        /// <param name="fail"></param>
        /// <param name="success"></param>
        public static void Perform(bool test, Action fail, Action success) {
            if (test) {
                success( );
                return;
            }
            fail( );
        }
        /// <summary>
        /// Call success if performing test returns true, otherwise call fail
        /// </summary>
        /// <param name="test"></param>
        /// <param name="fail"></param>
        /// <param name="success"></param>
        public static void Perform(Func<bool> test, Action fail, Action success)  => Perform(test(), fail, success);
        /// <summary>
        /// Call success when the result of test is true, otherwise do nothing.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="success"></param>
        public static void Perform(Func<bool> test, Action success) => Perform(test, () => { }, success);

        /// <summary>
        /// Call success if test is true
        /// </summary>
        /// <param name="test"></param>
        /// <param name="success"></param>
        public static void Perform(bool test, Action success) => Perform(test, () => { }, success);
        /// <summary>
        /// Call each function in order. 
        /// </summary>
        /// <param name="funcs"></param>
        public static void Compose(params Action[] funcs) {
            for (int i = 0 ; i < funcs.Length ; i++) {
                funcs[i].Invoke( );
            }
        }
        /// <summary>
        /// Throw the exception if test is true
        /// </summary>
        /// <param name="test"></param>
        /// <param name="e"></param>
        public static void ThrowIf(bool test, Exception e) => Perform(test, () => throw e );
        public static void ThrowIf(Func<bool> test, Exception e) => ThrowIf(test( ), e);

        public static void Nothing() {
        }
    }
}
