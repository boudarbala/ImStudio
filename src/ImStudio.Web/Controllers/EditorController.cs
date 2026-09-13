using ImStudio.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImStudio.Web.Controllers;

public sealed class EditorController(ILayoutService layoutService) : Controller
{
    public IActionResult Index() => View(layoutService.CreateDefault());
}
