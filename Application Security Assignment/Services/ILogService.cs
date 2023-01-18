using Application_Security_Assignment.Data.Enums;
using Application_Security_Assignment.Data.Models;
using System.Net;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public interface ILogService
    {
        public Task LogUser(Actions action, string userEmail);
        public Task<Result<List<Log>>> RetrieveLogs();

    }
}
