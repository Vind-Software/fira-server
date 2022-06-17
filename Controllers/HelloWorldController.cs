using Microsoft.AspNetCore.Mvc;

namespace fira_server.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloWorldController : ControllerBase {
    [HttpGet(Name = "GetHelloWorld")]
    public string Get() {
        Models.HelloWorld hw = new Models.HelloWorld();
        hw.content = "Hello World";
        return hw.content;
    }
}
