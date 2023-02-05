using Application_Security_Assignment.Filters;
using Application_Security_Assignment.Services;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Security_Assignment.Pages
{
    [Authorize(Roles = "Admin"), ServiceFilter(typeof(SecurityFilter))]
    public class LogsModel : PageModel
    {
        [BindProperty]
        public LogsUiState LogsUiState { get; set; } = new LogsUiState();
        private readonly ILogService _logService;
        public LogsModel(ILogService logService)
        {
            _logService = logService;
        }
        public void OnGet()
        {
            LogsUiState.Logs = _logService.RetrieveLogs().Result.Value;
        }
    }
}
