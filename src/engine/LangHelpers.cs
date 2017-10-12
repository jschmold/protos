using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Engine {

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
        public static void Nullify<Nullable>(Nullable arg) => arg = default;

        public static void Repeat(int amt, Action act) => Perform(amt != 0, () => {;
            for (int i = 0 ; i < amt ; i++) {
                act.Invoke();
            }
        });
        public static void Repeat(int amt, Action<int> act) => Perform(amt != 0, () => Repeat(amt, 0, act));
        public static void Repeat(int amt, int startIndex, Action<int> act) {
            for (int i = 0 ; i < amt ; i++) {
                act.Invoke(startIndex + i);
            }
        }
        public static uint Smallest(params uint[] nums) {
            ThrowIf(nums.Length == 0, new ArgumentOutOfRangeException("At least one argument required."));
            if (nums.Length == 1) {
                return nums[0];
            }
            uint smallest = nums[0];
            Repeat(nums.Length - 2, 1, ind => smallest = Math.Min(nums[ind], smallest));
            return smallest;
        }
        public static int Smallest(params int[] nums) {
            ThrowIf(nums.Length == 0, new ArgumentOutOfRangeException("At least one argument required."));
            if (nums.Length == 1) {
                return nums[0];
            }
            int smallest = nums[0];
            Repeat(nums.Length - 2, 1, ind => smallest = Math.Min(nums[ind], smallest));
            return smallest;
        }
        public static byte Smallest(params byte[] nums) {
            ThrowIf(nums.Length == 0, new ArgumentOutOfRangeException("At least one argument required"));
            if (nums.Length == 1) {
                return nums[0];
            }
            byte smallest = nums[0];
            Repeat(nums.Length - 2, 1, ind => smallest = Math.Max(nums[ind], smallest));
            return smallest;
        }
        public static double Smallest(params double[] nums) {
            ThrowIf(nums.Length == 0, new ArgumentOutOfRangeException("At least one argument required"));
            if (nums.Length == 1) {
                return nums[0];
            }
            double smallest = nums[0];
            Repeat(nums.Length - 2, 1, ind => smallest = Math.Max(nums[ind], smallest));
            return smallest;
        }
        public static float Smallest(params float[] nums) {
            ThrowIf(nums.Length == 0, new ArgumentOutOfRangeException("At least one argument required"));
            if (nums.Length == 1) {
                return nums[0];
            }
            float smallest = nums[0];
            Repeat(nums.Length - 2, 1, ind => smallest = Math.Max(nums[ind], smallest));
            return smallest;
        }
        public static T NullFn<T>() => default;
        
    }
}
