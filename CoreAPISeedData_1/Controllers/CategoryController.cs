﻿using CoreAPISeedData_1.Models.Categories.RequestModels;
using CoreAPISeedData_1.Models.Categories.ResponseModels;
using CoreAPISeedData_1.Models.Context;
using CoreAPISeedData_1.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CoreAPISeedData_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        MyContext _db;
        public CategoryController(MyContext db)
        {
            _db = db;
        }

        //Eger bir Action'a HTTPRequest tipini attribute olarak vermezseniz API ilgili Action isminin basında Get,Post,Put,Delete var mı diyece bakacaktır...Bunlardan birisini bulursa Request tipini o olarak algılayacaktır


        [HttpGet]

        public IActionResult GetCategories()
        {
            List<CategoryResponseModel> categories = _db.Categories.Select(x => new CategoryResponseModel
            {
                ID = x.ID,
                CategoryName = x.CategoryName,
                Description = x.Description
            }).ToList();

            return Ok(categories);
        }


        [HttpGet("id")] //api/Controller/id
        public async Task<IActionResult> GetCategory(int id)
        {
            CategoryResponseModel? category = await _db.Categories.Where(x => x.ID == id)
                .Select(x => new CategoryResponseModel
                {
                    ID = x.ID,
                    CategoryName = x.CategoryName,
                    Description = x.Description


                }).FirstOrDefaultAsync();


            return Ok(category);
        }


        [HttpPost]

        //Postman tarafında JSON veri yollayabilmek icin Post tarafında Raw'u secmeyi unutmayın...
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestModel model)
        {
            Category c = new()
            {
                CategoryName = model.CategoryName,
                Description = model.Description
            };

            await _db.Categories.AddAsync(c);
            await _db.SaveChangesAsync();
            return Ok(new CategoryResponseModel { ID = c.ID,CategoryName = c.CategoryName,Description = c.Description});

        }


        //Update

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequestModel model)
        {
            Category originalCategory = await _db.Categories.FindAsync(model.ID);
            originalCategory.CategoryName = model.CategoryName;
            originalCategory.Description = model.Description;
            await _db.SaveChangesAsync();
            return Ok(new CategoryResponseModel { ID = model.ID,CategoryName = model.CategoryName,Description = model.Description});
        }


        //Delete
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _db.Categories.Remove(await _db.Categories.FindAsync(id));
            await _db.SaveChangesAsync();

            return Ok("Silme işlemi basarılı bir şekilde gercekleşti");
        }
    }
}
