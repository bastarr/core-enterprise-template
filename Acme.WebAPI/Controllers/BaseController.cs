using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Acme.Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EFCore.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly ILogger Logger;

        public BaseController(IUnitOfWork unitOfWork, ILogger logger)
        {
            UnitOfWork = unitOfWork;
            Logger = logger;
        }

        /// <summary>
        /// Executes and operation with common error handling and processing
        /// </summary>
        /// <typeparam name="T">Type to returned from the function</typeparam>
        /// <param name="codeToExecute">Function to execute</param>
        /// <returns></returns>
        protected T ExecuteHandledOperation<T>(Func<T> codeToExecute)
        {
            try
            {
                var timer = Stopwatch.StartNew();

                var method = codeToExecute.Method.Name;

                Logger.LogInformation($"{method} starting with user context...");

                T result = codeToExecute.Invoke();

                Logger.LogInformation($"{method} completed. duration: {timer.Elapsed}");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.StackTrace);
                throw;
            }

        }

                /// <summary>
        /// Executes and operation with common error handling and processing
        /// </summary>
        /// <typeparam name="T">Type to returned from the function</typeparam>
        /// <param name="codeToExecute">Function to execute</param>
        /// <returns></returns>
        protected async Task<T> ExecuteHandledOperationAsync<T>(Func<Task<T>> codeToExecute, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var timer = Stopwatch.StartNew();

                var method = codeToExecute.Method.Name;

                Logger.LogInformation($"{method} starting with user context...");

                T result = await Task.Run(() => codeToExecute.Invoke());

                Logger.LogInformation($"{method} completed. duration: {timer.Elapsed}");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.StackTrace);
                throw;
            }

        }
    }
}