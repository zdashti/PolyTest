// See https://aka.ms/new-console-template for more information

using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PollyTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var _url = "https://jsonplaceholder.typicode.com/posts";
            var _url1 = "https://jsonplaceholder.typicode.com/posts1";
            var _url2 = "fake";

            IReTryableService<HttpResponseMessage> service = new ReTryableService<HttpResponseMessage>();

            var test = await
               service.WaitAndRetryForeverWithConditionAndExceptionFromSeconds
                    (_url, 3, (new HttpClient()).GetAsync, ResultPredicate());

            Console.ReadLine();
        }

        private static Func<HttpResponseMessage, bool> ResultPredicate()
        {
            return res => !res.IsSuccessStatusCode;
        }

        #region Retry
        private static async Task Retry(string url, int retry)
        {
            var policy = Policy.Handle<Exception>().RetryAsync(retry);

            await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                await (new HttpClient()).GetAsync(url);
            });
        }

        private static async Task RetryWithCondition(string url, int retry)
        {
            var policy = Policy.HandleResult<HttpResponseMessage>
                (ResultPredicate()).RetryAsync(retry);

            var res = await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                return await new HttpClient().GetAsync(url);
            });
        }

        private static async Task RetryWithConditionAndException(string url, int retry)
        {
            var policy = Policy.Handle<Exception>()
                .OrResult<HttpResponseMessage>(ResultPredicate())
                .RetryAsync(retry);

            var res = await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                return await new HttpClient().GetAsync(url);
            });
        }
        #endregion

        #region WaitAndRetry
        private static async Task WaitAndRetryFromSeconds(string url, int retry, int waiting)
        {
            var policy = Policy.Handle<Exception>().WaitAndRetryAsync(retry, wait => TimeSpan.FromSeconds(waiting));

            await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                await (new HttpClient()).GetAsync(url);
            });
        }

        private static async Task WaitAndRetryWithConditionFromSeconds(string url, int retry, int waiting)
        {
            var policy = Policy.HandleResult<HttpResponseMessage>
                    (ResultPredicate())
                .WaitAndRetryAsync(retry, wait => TimeSpan.FromSeconds(waiting));

            var res = await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                return await new HttpClient().GetAsync(url);
            });
        }

        private static async Task WaitAndRetryWithConditionAndExceptionFromSeconds(string url, int retry, int waiting)
        {
            var policy = Policy.Handle<Exception>()
                .OrResult<HttpResponseMessage>(ResultPredicate())
                .WaitAndRetryAsync(retry, wait => TimeSpan.FromSeconds(waiting));

            var res = await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                return await new HttpClient().GetAsync(url);
            });
        }
        #endregion

        #region WaitAndRetryForever
        private static async Task WaitAndRetryForeverFromSeconds(string url, int waiting)
        {
            var policy = Policy.Handle<Exception>().WaitAndRetryForeverAsync(wait => TimeSpan.FromSeconds(waiting));

            await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                await (new HttpClient()).GetAsync(url);
            });
        }

        private static async Task WaitAndRetryForeverWithConditionFromSeconds(string url, int retry, int waiting)
        {
            var policy = Policy.HandleResult<HttpResponseMessage>
                    (ResultPredicate())
                .WaitAndRetryForeverAsync(wait => TimeSpan.FromSeconds(waiting));

            var res = await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                return await new HttpClient().GetAsync(url);
            });
        }

        private static async Task WaitAndRetryForeverWithConditionAndExceptionFromSeconds(string url, int waiting)
        {
            var policy = Policy.Handle<Exception>()
                .OrResult<HttpResponseMessage>(ResultPredicate())
                .WaitAndRetryForeverAsync(wait => TimeSpan.FromSeconds(waiting));

            var res = await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ..."+DateTime.Now);
                return await new HttpClient().GetAsync(url);
            });
        }
        #endregion

        #region CircuitBreaker
        private static async Task CircuitBreakerFromSeconds(string url, int retry, int waiting)
        {
            var policy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(retry, TimeSpan.FromSeconds(waiting));

            await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                await (new HttpClient()).GetAsync(url);
            });
        }

        private static async Task CircuitBreakerWithConditionFromSeconds(string url, int retry, int waiting)
        {
            var policy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(retry, TimeSpan.FromSeconds(waiting));

            await policy.ExecuteAsync(async () =>
            {

                Console.WriteLine("retrying ci ...");
                var tert = await new HttpClient().GetAsync(url);
                return tert;
            });
        }

        private static async Task CircuitBreakerWithConditionAndExceptionFromSeconds(string url, int retry, int waiting)
        {
            var policy = Policy.Handle<Exception>()
                .OrResult<HttpResponseMessage>(ResultPredicate())
                .CircuitBreakerAsync(retry, TimeSpan.FromSeconds(waiting));

            var res = await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                return await new HttpClient().GetAsync(url);
            });
        }
        #endregion

        #region AdvancedCircuitBreaker
        private static async Task AdvancedCircuitBreakerFromSeconds(string url, double failureThreshold, int samplingDuration, int minimumThroughput, int durationOfBreak)
        {
            var policy = Policy.Handle<Exception>()
                .AdvancedCircuitBreakerAsync(failureThreshold, TimeSpan.FromSeconds(samplingDuration), minimumThroughput,
                    TimeSpan.FromSeconds(durationOfBreak));

            await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                await (new HttpClient()).GetAsync(url);
            });
        }

        private static async Task AdvancedCircuitBreakerWithConditionFromSeconds(string url, double failureThreshold, int samplingDuration, int minimumThroughput, int durationOfBreak)
        {
            var policy = Policy
                .HandleResult<HttpResponseMessage>(ResultPredicate())
                .AdvancedCircuitBreakerAsync(failureThreshold, TimeSpan.FromSeconds(samplingDuration), minimumThroughput,
                    TimeSpan.FromSeconds(durationOfBreak));

            await policy.ExecuteAsync(async () =>
                await (new HttpClient()).GetAsync(url));
        }

        private static async Task AdvancedCircuitBreakerWithConditionAndExceptionFromSeconds(string url, double failureThreshold, int samplingDuration, int minimumThroughput, int durationOfBreak)
        {
            var policy = Policy.Handle<Exception>()
                .OrResult<HttpResponseMessage>(ResultPredicate())
                .AdvancedCircuitBreakerAsync(failureThreshold, TimeSpan.FromSeconds(samplingDuration), minimumThroughput,
                    TimeSpan.FromSeconds(durationOfBreak));

            var res = await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("retrying ...");
                return await new HttpClient().GetAsync(url);
            });
        }
        #endregion
    }
}