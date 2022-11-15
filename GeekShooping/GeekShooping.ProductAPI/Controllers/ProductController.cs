﻿using GeekShooping.ProductAPI.Data.ValueObjects;
using GeekShooping.ProductAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekShooping.ProductAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _repository;
        public ProductController(IProductRepository repository)
        {
            _repository= repository ?? throw new ArgumentNullException(nameof(repository));

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVO>> FindById(long id)
        {
            var product = await _repository.FindByID(id);
            if (product.Id <= 0) return NotFound();
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll()
        {
            var product = await _repository.FindAll();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductVO>> Create(ProductVO vo)
        {
           if (vo == null) return BadRequest();
           var product = await _repository.Create(vo);
           return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult<ProductVO>> Update(ProductVO vo)
        {
            if (vo == null) return BadRequest();
            var product = await _repository.Update(vo);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var status = await _repository.Delete(id);
            if (!status) return BadRequest();
            return Ok(status);
        }
    }
}