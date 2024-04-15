using Microsoft.AspNetCore.Mvc;

namespace starter_template.Controllers;

    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public string GetTest()
        {
            return "hello";
        }
    }
