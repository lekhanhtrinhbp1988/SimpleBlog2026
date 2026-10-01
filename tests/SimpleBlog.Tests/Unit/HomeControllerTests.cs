using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Web.Controllers;

namespace SimpleBlog.Tests.Unit;

public class HomeControllerTests
{
    [Fact]
    public void About_ReturnsViewResult()
    {
        var result = new HomeController().About();

        var view = Assert.IsType<ViewResult>(result);
        Assert.Null(view.ViewName);
    }
}
