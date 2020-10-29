using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Roulette.Filters
{
    public class ServerErrorResult : ObjectResult
    {
        public ServerErrorResult(string message) : base(message)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
