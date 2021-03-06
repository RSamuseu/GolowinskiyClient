﻿using Microsoft.AspNetCore.Mvc;
using GolovinskyAPI.Data.Interfaces;
using GolovinskyAPI.Logic.Interfaces;

namespace GolovinskyAPI.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/shopinfo")]
    public class UrlController : ControllerBase
    {
        private readonly IRepository repo;
        private readonly ICustomizeService service;

        public UrlController(IRepository repository, ICustomizeService serv)
        {
            repo = repository;
            service = serv;
        }

        /// <summary>
        /// Отображение информации о магазине
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet("{url}")]
        public IActionResult Get(string url)
        {
            var mainImage = service.GetMainImage();
            var accountMainImage = service.GetMainImageUserAccount();
            var res = repo.GetSubDomain(url);
            if (res == null)
            {
                return NotFound(new { 
                    Message = $"Извините, магазин {url}.головинский.рф не найден", 
                    MainPicture = $"/mainimages/{mainImage}",
                    MainPictureAccountUser = $"/accountImages/{accountMainImage}",
                Status = false });
            }
            res.MainPicture = $"/mainimages/{mainImage}";
            res.MainPictureAccountUser = $"/accountImages/{accountMainImage}";

            return Ok(new { 
                cust_id = res.cust_id, 
                mainImage = res.MainPicture,
                mainPictureAccountUser = res.MainPictureAccountUser,
                email = res.e_mail,
                dz = res.DZ,
                phone = res.phone
            });
        }
    }
}