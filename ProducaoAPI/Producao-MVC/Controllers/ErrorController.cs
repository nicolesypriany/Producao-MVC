using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    public IActionResult GeneralError()
    {
        var errorMessage = Request.Query["errorMessage"].ToString();
        ViewData["ErrorMessage"] = errorMessage;
        return View("GeneralError");
    }
}
