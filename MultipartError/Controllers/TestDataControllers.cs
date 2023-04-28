using Microsoft.AspNetCore.Mvc;
using MultipartError.Binders;
using MultipartError.Models;
using NLog;

namespace MultipartError.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestDataControllers : ControllerBase
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost("test")]
        public async Task<IActionResult> TestData(
            [ModelBinder(BinderType = typeof(JsonModelBinder))] TestDataModel request,
            [ModelBinder(BinderType = typeof(FileModelBinder))] IList<IFormFile> files,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                this.logger.Info($"{nameof(TestData)} вернул null");
                return this.BadRequest();
            }

            this.logger.Info($"{nameof(TestData)} вернул модуль");
            return this.Ok();
        }
    }
}