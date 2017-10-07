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

        public static void DoOrThrow(bool test, Action act, Exception e) => Perform(test, () => DoOrThrow(act, e));

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
        /// Call success if true, or call DoOrThrow if false using the tuple fail's values for the DoOrThrow
        /// </summary>
        /// <param name="test">Whether or not to perform success</param>
        /// <param name="fail">fail.fail is the action to call, if null, fail.e will be used in DoOrThrow</param>
        /// <param name="success">What to do if the test passes</param>
        public static void Perform(bool test, (Action fail, Exception e) fail, Action success) => Perform(test, () => DoOrThrow(fail.fail, fail.e), success);

        /// <summary>
        /// Call success if true, or call DoOrThrow if false using the tuple fail's values for the DoOrThrow
        /// </summary>
        /// <param name="test">Whether or not to perform success</param>
        /// <param name="fail">fail.fail is the action to call, if null, fail.e will be used in DoOrThrow</param>
        /// <param name="success">What to do if the test passes</param>
        /// <param name="success"></param>
        public static void Perform(Func<bool> test, (Action fail, Exception e) fail, Action success) => Perform(test( ), fail, success);

        /// <summary>
        /// Throw an exception if the test fails, otherwise perform success
        /// </summary>
        /// <param name="test"></param>
        /// <param name="fail"></param>
        /// <param name="success"></param>
        public static void Perform(bool test, Exception fail, Action success) => Perform(test, () => throw fail, success);

        /// <summary>
        /// Throw an exception if the test fails, otherwise perform success
        /// </summary>
        /// <param name="test"></param>
        /// <param name="fail"></param>
        /// <param name="success"></param>
        public static void Perform(Func<bool> test, Exception fail, Action success) => Perform(test( ), () => throw fail, success);
        /// <summary>
        /// Call success if performing test returns true, otherwise call fail
        /// </summary>
        /// <param name="test"></param>
        /// <param name="fail"></param>
        /// <param name="success"></param>
        public static void Perform(Func<bool> test, Action fail, Action success) => Perform(test( ), fail, success);
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

        public static void Compose(bool test, params Action[] funcs) => Perform(test, () => {
            for (int i = 0 ; i < funcs.Length ; i++) {
                funcs[i].Invoke( );
            }
        });
        /// <summary>
        /// Throw the exception if test is true
        /// </summary>
        /// <param name="test"></param>
        /// <param name="e"></param>
        public static void ThrowIf(bool test, Exception e) => Perform(test, () => throw e);
        public static void ThrowIf(Func<bool> test, Exception e) => ThrowIf(test( ), e);

        public static void DoNothing() {
        }

        /// <summary>
        /// Perform a set of operations on an object of T, passing the result to the next function in the <paramref name="pipeline"/>.
        /// </summary>
        /// <typeparam name="T">The type to use</typeparam>
        /// <param name="arg">The item to operate on</param>
        /// <param name="pipeline">The array of functions to iterate over</param>
        /// <returns></returns>
        public static T Pipe<T>(T arg, params Func<T, T>[] pipeline) {
            foreach (var func in pipeline) {
                arg = func(arg);
            }
            return arg;
        }
        /// <summary>
        /// Perform a set of operations on an object of T
        /// </summary>
        /// <typeparam name="T">The type to use</typeparam>
        /// <param name="arg">The item to operate on</param>
        /// <param name="pipeline">The array of functions to iterate over</param>
        public static void Compose<T>(T arg, params Action<T>[] pipeline) {
            foreach (var func in pipeline) {
                func(arg);
            }
        }
        /// <summary>
        /// Perform a set of operations if test is true on an object of T
        /// </summary>
        /// <typeparam name="T">The type to use</typeparam>
        /// <param name="test">Whether or not the pipeline should be executed</param>
        /// <param name="arg">The item to operate on</param>
        /// <param name="pipeline">The array of functions to iterate over</param>
        public static void Compose<T>(bool test, T arg, params Action<T>[] pipeline) => Perform(test, () => {
            foreach (var func in pipeline) {
                func(arg);
            }
        });
        public static void Nullify<Nullable>(Nullable arg) => arg = default(Nullable);
    }
}
