using AutoMapper;
using GolovinskyAPI.Data.Interfaces;
using GolovinskyAPI.Data.Models.Background;
using GolovinskyAPI.Logic.Interfaces;
using GolovinskyAPI.Logic.Models;
using GolovinskyAPI.Logic.Models.Background;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GolovinskyAPI.Web.Controllers
{
    /// <summary>
    /// Контроллер для работы с картинками
    /// </summary>
    /// <returns></returns>
    //[Produces("application/json")]
    [Route("api/Background")]

    public class BackgroundController : Controller
    {
        private readonly IBackgroundRepository _repo;
        private readonly IUploadPicture _uploadHandler;
        private readonly IMapper _autoMapper;

        public BackgroundController(
            IBackgroundRepository repo,
            IUploadPicture uploadHandler,
            IMapper autoMapper)
        {
            _repo = repo;
            _uploadHandler = uploadHandler;
            _autoMapper = autoMapper;
        }

        /// <summary>
        /// Получение фона в формате base64
        /// </summary>
        [HttpGet]
        [Route("base64")]
        public async Task<IActionResult> Get(string appCode, char mark, char orientation, char place)
        {
            var currentTime = DateTime.Now;

            var background = new Background()
            {
                AppCode = appCode,
                Date = currentTime,
                Mark = mark,
                orient = orientation,
                place = place
            };

            var result = await _repo.GetBackground(background);

            ViewBag.Backgrounds = result;

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var response = result.Select(background =>
                    new
                    {
                        Image = _uploadHandler.GetBase64Image(background.Img),
                        FileName = background.FileName,
                        Orientation = background.orient,
                        Place = background.place
                    }).ToList();
                return Ok(response);
            }
        }

        /// <summary>
        /// Добавление фона в формате base64
        /// </summary>
        [HttpPost]
        [Route("base64")]
        public async Task<IActionResult> Post([FromForm] BackgroundPostBase64ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Не верные параметры в запросе");
            }

            var currentTime = DateTime.Now;
            var bytesImage = _uploadHandler.UplodaBase64Image(model.Image);

            var background = _autoMapper.Map<Background>(model);
            background.Img = bytesImage;
            background.Date = currentTime;

            var response = await _repo.Create(background);

            return Ok(new ResponseViewModel { Response = response });
        }

        /// <summary>
        /// Обновление фона в формате base64
        /// </summary>
        [HttpPut]
        [Route("base64")]
        public async Task<IActionResult> Put([FromForm] BackgroundPutBase64ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Не верные параметры в запросе");
            }

            var currentTime = DateTime.Now;
            var bytesImage = _uploadHandler.UplodaBase64Image(model.Image);

            var background = _autoMapper.Map<Background>(model);
            background.Img = bytesImage;
            background.Date = currentTime;

            var response = await _repo.Update(background);

            return Ok(new ResponseViewModel { Response = response });
        }

        /// <summary>
        /// Получение фона в file формате
        /// </summary>
        [HttpGet]
        [Route("file")]
        public async Task<IActionResult> Get(char place, string appCode, char mark, char orientation)
        {
            var currentTime = DateTime.Now;

            var background = new Background()
            {
                AppCode = appCode,
                Date = currentTime,
                Mark = mark,
                orient = orientation,
                place = place
            };

            var result = await _repo.GetBackground(background);

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var response = result.Select(background =>
                    new
                    {
                        FileName = background.FileName,
                        Orientation = background.orient,
                        Place = background.place
                    }).ToList();
                return Ok(response);
            }
        }

        /// <summary>
        /// Добавление фона в file формате
        /// </summary>
        [HttpPost]
        [Route("file")]
        public async Task<IActionResult> Post([FromForm] BackgroundPostFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Не верные параметры в запросе");
            }

            var currentTime = DateTime.Now;
            var bytesImage = await _uploadHandler.UploadPicture(model.AppCode, model.Image);

            var background = _autoMapper.Map<Background>(model);
            background.Img = bytesImage;
            background.Date = currentTime;

            var response = await _repo.Create(background);

            return Ok(new ResponseViewModel { Response = response });
        }

        /// <summary>
        /// Обновление фона в file формате
        /// </summary>
        [HttpPut]
        [Route("file")]
        public async Task<IActionResult> Put([FromForm] BackgroundPutFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Не верные параметры в запросе");
            }

            var currentTime = DateTime.Now;
            var bytesImage = await _uploadHandler.UploadPicture(model.AppCode, model.Image);

            var background = _autoMapper.Map<Background>(model);
            background.Img = bytesImage;
            background.Date = currentTime;

            var response = await _repo.Update(background);

            return Ok(new ResponseViewModel { Response = response });
        }

        /// <summary>
        /// Удаление фона формата base64
        /// </summary>
        [HttpDelete]
        [Route("base64")]
        public async Task<IActionResult> Delete([FromBody] BackgroundBase64DeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Не верные параметры в запросе");
            }

            var background = _autoMapper.Map<Background>(model);

            var response = await _repo.Delete(background);

            if (response == "1")
                return Ok(new ResponseViewModel { Response = response });
            else
                return BadRequest(new ResponseViewModel { Response = "Ошибка выполнения процедуры" });
        }
    }
}
