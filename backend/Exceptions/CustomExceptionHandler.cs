using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PalBet.Exceptions
{
    [Serializable]
    public class CustomException : Exception
    {
        public string Error { get; }
       
        public int ErrorCode { get;  } 

        public CustomException(string error, string message, int errorCode) : base(message)
        {
            Error = error;
            ErrorCode = errorCode;
        }
    }

    public class CustomExceptionHandler : IExceptionHandler
    {

        private readonly IProblemDetailsService _problemDetailService;
        public CustomExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailService = problemDetailsService;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            
            if (exception is not CustomException customException)
            {
                return true;
            }
            Console.WriteLine("Caught a custom exception");
            var customError = new ProblemDetails
            {
                Status = customException.ErrorCode,
                Title = customException.Error,
                Detail = customException.Message,
                Type = "Bad Request",
            };
            httpContext.Response.StatusCode = (int) customError.Status;
            return await _problemDetailService.TryWriteAsync(new ProblemDetailsContext { HttpContext = httpContext, ProblemDetails = customError });



        }
    }
}
