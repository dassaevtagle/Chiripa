using System;
using System.Collections.Generic;
using AutoMapper;
using ChiripaAPI.Dtos;
using ChiripaAPI.Models;
using ChiripaAPI.Services.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ChiripaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class HielitosController : ControllerBase
    {
        private readonly IHielito repoHielito;
        private readonly IMapper mapper;

        public HielitosController(IHielito repoHielito, IMapper mapper)
        {
            this.repoHielito = repoHielito;
            this.mapper = mapper;
        }

        //GET api/hielitos/obtener
        [HttpGet]
        public IActionResult Obtener()
        {
            try
            {
                var hielitos = repoHielito.GetAllHielitos();

                return Ok(mapper.Map<List<HielitoReadDto>>(hielitos));
            }

            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //GET api/hielitos/obtener/{id}
        [HttpGet("{id}", Name="Obtener")]
        public IActionResult Obtener(int id)
        {
            try
            {
                var hielito = repoHielito.GetHielitoById(id);
                if(hielito == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<HielitoReadDto>(hielito));
            }

            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //POST api/hielitos/guardar
        [HttpPost]
        public IActionResult Guardar([FromBody]HielitoCreateDto hielitoCreateDto)
        {
            try
            {
                var hielitoModel = mapper.Map<Hielito>(hielitoCreateDto);

                var newHielito = repoHielito.AddNewHielito(hielitoModel);
                if(newHielito == null)
                {
                    return BadRequest("That name already exists!");
                }

                var hielitoReadDto = mapper.Map<HielitoReadDto>(newHielito);

                return CreatedAtRoute(nameof(Obtener), new {Id = hielitoReadDto.Id}, hielitoReadDto);
            }

            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //PUT api/hielitos/editar/{id}

        [HttpPut("{id}")]
        public IActionResult Editar([FromBody]HielitoUpdateDto hielitoUpdateDto, int id)
        {   
            try
            {
                var hielitoFromRepo = repoHielito.GetHielitoById(id);
                if(hielitoFromRepo == null)
                {
                    return NotFound();
                }
               
                mapper.Map(hielitoUpdateDto, hielitoFromRepo);
                
                repoHielito.UpdateHielito(hielitoFromRepo);

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //PATCH  api/hielitos/parchear/{id}
        [HttpPatch("{id}")]
        public IActionResult Parchear(int id, JsonPatchDocument<HielitoUpdateDto> patchDocument) 
        {
            try
            {
                var hielitoFromRepo = repoHielito.GetHielitoById(id);
                if(hielitoFromRepo == null)
                {
                    return NotFound();
                }
               
                var hielitoToPatch = mapper.Map<HielitoUpdateDto>(hielitoFromRepo);
                patchDocument.ApplyTo(hielitoToPatch, ModelState);
                
                if(!TryValidateModel(hielitoToPatch))
                {
                    return ValidationProblem(ModelState);
                }

                mapper.Map(hielitoToPatch, hielitoFromRepo);

                repoHielito.UpdateHielito(hielitoFromRepo);

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                var hielito = repoHielito.GetHielitoById(id);
                
                if(hielito == null)
                {
                    return NotFound();
                }

                repoHielito.DeleteHielito(id);
                
                return Ok(repoHielito.GetAllHielitos());
            }
            
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}