using System;
using System.Threading.Tasks;

namespace AmpedBiz.Common.Extentions
{
    public static class TaskExtention
    {
        public static async Task<(T1, T2)> WhenAll<T1, T2>(Task<T1> task1, Task<T2> task2)
        {
            await Task.WhenAll(task1, task2);
            return (task1.Result, task2.Result);
        }

        public static async Task<TResult> WhenAll<T1, T2, TResult>(Task<T1> task1, Task<T2> task2, Func<T1, T2, TResult> result)
        {
            await Task.WhenAll(task1, task2);
            return result(task1.Result, task2.Result);
        }

        public static async Task<(T1, T2, T3)> WhenAll<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Task<T3> task3)
        {
            await Task.WhenAll(task1, task2, task3);
            return (task1.Result, task2.Result, task3.Result);
        }

        public static async Task<TResult> WhenAll<T1, T2, T3, TResult>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Func<T1, T2, T3, TResult> result)
        {
            await Task.WhenAll(task1, task2, task3);
            return result(task1.Result, task2.Result, task3.Result);
        }

        public static async Task<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4)
        {
            await Task.WhenAll(task1, task2, task3, task4);
            return (task1.Result, task2.Result, task3.Result, task4.Result);
        }

        public static async Task<TResult> WhenAll<T1, T2, T3, T4, TResult>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Func<T1, T2, T3, T4, TResult> result)
        {
            await Task.WhenAll(task1, task2, task3, task4);
            return result(task1.Result, task2.Result, task3.Result, task4.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5)> WhenAll<T1, T2, T3, T4, T5>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5)
        {
            await Task.WhenAll(task1, task2, task3, task4, task5);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result);
        }

        public static async Task<TResult> WhenAll<T1, T2, T3, T4, T5, TResult>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Func<T1, T2, T3, T4, T5, TResult> result)
        {
            await Task.WhenAll(task1, task2, task3, task4, task5);
            return result(task1.Result, task2.Result, task3.Result, task4.Result, task5.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6)> WhenAll<T1, T2, T3, T4, T5, T6>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6)
        {
            await Task.WhenAll(task1, task2, task3, task4, task5, task6);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result);
        }

        public static async Task<TResult> WhenAll<T1, T2, T3, T4, T5, T6, TResult>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Func<T1, T2, T3, T4, T5, T6, TResult> result)
        {
            await Task.WhenAll(task1, task2, task3, task4, task5, task6);
            return result(task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6, T7)> WhenAll<T1, T2, T3, T4, T5, T6, T7>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7)
        {
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7);
            return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result);
        }

        public static async Task<TResult> WhenAll<T1, T2, T3, T4, T5, T6, T7, TResult>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> result)
        {
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7);
            return result(task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result);
        }
    }
}
