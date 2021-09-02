using CORE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADONET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personrepository;
        private readonly IConfiguration _configuration;


        public PersonController(IPersonRepository personRepository, IConfiguration configuration)
        {
            _personrepository = personRepository;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            var person = _personrepository.GetPersonByIdAsync(id);
            return Ok(person);
        }
        [HttpGet]
        public ActionResult<List<Person>> Get()
        {
            var persons = _personrepository.GetPersonsAsync();
            return Ok(persons);
        }

        [HttpPost]

        public ActionResult<int> Post(Person person)
        {
           return _personrepository.CreatePersonAsync(person);
        }

        [HttpPut]

        public ActionResult Update(Person person)
        {
            _personrepository.UpdatePersonAsync(person);
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _personrepository.DeletePersonAsync(id);
            return Ok();
        }
    }
}
