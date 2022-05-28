// See https://aka.ms/new-console-template for more information

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
                    (_url, 3, (new HttpClient()).GetAsync, res => !res.IsSuccessStatusCode);

            Console.ReadLine();
        }
    }
}