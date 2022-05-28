using System;
using System.Threading.Tasks;

namespace PollyTest
{
    public interface IReTryableService<T>
    {
        Task<T> Retry(string url, int retry, Func<string, Task<T>> func);
        Task<T> RetryWithCondition(string url, int retry, Func<string, Task<T>> func, Func<T, bool> cond);
        Task<T> RetryWithConditionAndException(string url, int retry, Func<string, Task<T>> func,
             Func<T, bool> cond);
        
        Task<T> WaitAndRetryFromSeconds(string url, int retry, int waiting, Func<string, Task<T>> func);
        Task<T> WaitAndRetryWithConditionFromSeconds(string url, int retry, int waiting,
             Func<string, Task<T>> func, Func<T, bool> cond);
        Task<T> WaitAndRetryWithConditionAndExceptionFromSeconds(string url, int retry, int waiting,
           Func<string, Task<T>> func, Func<T, bool> cond);

        Task<T> WaitAndRetryForeverFromSeconds(string url, int waiting, Func<string, Task<T>> func);
        Task<T> WaitAndRetryForeverWithConditionFromSeconds(string url, int waiting,
            Func<string, Task<T>> func, Func<T, bool> cond);
        Task<T> WaitAndRetryForeverWithConditionAndExceptionFromSeconds(string url, int waiting,
            Func<string, Task<T>> func, Func<T, bool> cond);

        Task<T> CircuitBreakerFromSeconds(string url, int retry, int waiting, Func<string, Task<T>> func);
        Task<T> CircuitBreakerWithConditionFromSeconds(string url, int retry, int waiting, Func<string, Task<T>> func,
            Func<T, bool> cond);
        Task<T> CircuitBreakerWithConditionAndExceptionFromSeconds(string url, int retry, int waiting,
            Func<string, Task<T>> func, Func<T, bool> cond);
        
        Task<T> AdvancedCircuitBreakerFromSeconds(string url, double failureThreshold, int samplingDuration,
            int minimumThroughput, int durationOfBreak, Func<string, Task<T>> func);
        Task<T> AdvancedCircuitBreakerWithConditionFromSeconds(string url, double failureThreshold,
            int samplingDuration, int minimumThroughput, int durationOfBreak, Func<string, Task<T>> func,
            Func<T, bool> cond);
        Task<T> AdvancedCircuitBreakerWithConditionAndExceptionFromSeconds(string url, double failureThreshold,
            int samplingDuration, int minimumThroughput, int durationOfBreak, Func<string, Task<T>> func,
            Func<T, bool> cond);
    }
}
