using Polly;
using System;
using System.Threading.Tasks;

namespace PollyTest
{
    public class ReTryableService<T> : IReTryableService<T>
    {
        #region Retry
        public async Task<T> Retry(string url, int retry, Func<string, Task<T>> func)
        {
            return await Policy
                .Handle<Exception>()
                .RetryAsync(retry)
                .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> RetryWithCondition(string url, int retry, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy
                .HandleResult
                (cond).RetryAsync(retry)
                .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> RetryWithConditionAndException(string url, int retry, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy
               .Handle<Exception>()
               .OrResult(cond)
               .RetryAsync(retry).ExecuteAsync(async () => await func(url));
        }
        #endregion
        #region WaitAndRetry
        public async Task<T> WaitAndRetryFromSeconds(string url, int retry, int waiting, Func<string, Task<T>> func)
        {
            return await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(retry, wait => TimeSpan.FromSeconds(waiting))
                .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> WaitAndRetryWithConditionFromSeconds(string url, int retry, int waiting, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy
                .HandleResult(cond)
                .WaitAndRetryAsync(retry, wait => TimeSpan.FromSeconds(waiting))
                .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> WaitAndRetryWithConditionAndExceptionFromSeconds(string url, int retry, int waiting, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy
                .Handle<Exception>()
                .OrResult(cond)
                .WaitAndRetryAsync(retry, wait => TimeSpan.FromSeconds(waiting))
                .ExecuteAsync(async () => await func(url));
        }
        #endregion
        #region WaitAndRetryForever
        public async Task<T> WaitAndRetryForeverFromSeconds(string url, int waiting, Func<string, Task<T>> func)
        {
            return await Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync( wait => TimeSpan.FromSeconds(waiting))
                .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> WaitAndRetryForeverWithConditionFromSeconds(string url, int waiting, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy
                .HandleResult(cond)
                .WaitAndRetryForeverAsync( wait => TimeSpan.FromSeconds(waiting))
                .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> WaitAndRetryForeverWithConditionAndExceptionFromSeconds(string url, int waiting, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy
                .Handle<Exception>()
                .OrResult(cond)
                .WaitAndRetryForeverAsync( wait => TimeSpan.FromSeconds(waiting))
                .ExecuteAsync(async () => await func(url));
        }
        #endregion
        #region CircuitBreaker
        public async Task<T> CircuitBreakerFromSeconds(string url, int retry, int waiting, Func<string, Task<T>> func)
        {
            return await Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(retry, TimeSpan.FromSeconds(waiting))
                .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> CircuitBreakerWithConditionFromSeconds(string url, int retry, int waiting, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy
                .HandleResult(cond)
                .CircuitBreakerAsync(retry, TimeSpan.FromSeconds(waiting))
                .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> CircuitBreakerWithConditionAndExceptionFromSeconds(string url, int retry, int waiting, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy.Handle<Exception>()
                .OrResult(cond)
                .CircuitBreakerAsync(retry, TimeSpan.FromSeconds(waiting))
                .ExecuteAsync(async () => await func(url));
        }
        #endregion
        #region AdvancedCircuitBreaker
        public async Task<T> AdvancedCircuitBreakerFromSeconds(string url, double failureThreshold, int samplingDuration, int minimumThroughput, int durationOfBreak, Func<string, Task<T>> func)
        {
            return await Policy.Handle<Exception>()
                .AdvancedCircuitBreakerAsync(failureThreshold, TimeSpan.FromSeconds(samplingDuration), minimumThroughput,
                    TimeSpan.FromSeconds(durationOfBreak))
                .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> AdvancedCircuitBreakerWithConditionFromSeconds(string url, double failureThreshold, int samplingDuration, int minimumThroughput, int durationOfBreak, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy
                 .HandleResult(cond)
                 .AdvancedCircuitBreakerAsync(failureThreshold, TimeSpan.FromSeconds(samplingDuration), minimumThroughput,
                    TimeSpan.FromSeconds(durationOfBreak))
                 .ExecuteAsync(async () => await func(url));
        }

        public async Task<T> AdvancedCircuitBreakerWithConditionAndExceptionFromSeconds(string url, double failureThreshold, int samplingDuration, int minimumThroughput, int durationOfBreak, Func<string, Task<T>> func, Func<T, bool> cond)
        {
            return await Policy.Handle<Exception>()
                .OrResult<T>(cond)
                .AdvancedCircuitBreakerAsync(failureThreshold, TimeSpan.FromSeconds(samplingDuration), minimumThroughput,
                    TimeSpan.FromSeconds(durationOfBreak)).ExecuteAsync(async () => await func(url));
        }
        #endregion
    }
}
