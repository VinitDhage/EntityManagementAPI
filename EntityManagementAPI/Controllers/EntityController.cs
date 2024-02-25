using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntityManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private static List<Entity> _entities = new List<Entity>
            {
                new Entity
                {
                    Id = "1",
                    Addresses = new List<Address>
                    {
                        new Address {AddressLine = "123 Main St",City = "Anytown", Country = "USA" }
                    },
                    Dates = new List<Date>
                    {
                        new Date { DateType = "Birth", DateValue = new DateTime(1990,1,1) }
                    },
                    Deceased = false,
                    Gender = "Male",
                    Names = new List<Name>
                    {
                        new Name { FirstName = "John",MiddleName = "Benjamin",Surname = "Doe" }
                    }
                },
                new Entity
                {
                    Id = "2",
                    Addresses = new List<Address>
                    {
                        new Address { AddressLine = "456 Oak St", City = "Smalltown", Country = "Canada" }
                    },
                    Dates = new List<Date>
                    {
                        new Date { DateType = "Birth", DateValue = new DateTime(1985, 5, 10) }
                    },
                    Deceased = false,
                    Gender = "Female",
                    Names = new List<Name>
                    {
                        new Name { FirstName = "Jane",MiddleName = "Ethan", Surname = "Smith" }
                    }
                },
                new Entity
                {
                    Id = "3",
                    Addresses = new List<Address>
                    {
                        new Address { AddressLine = "789 Elm St", City = "Villagetown", Country = "USA" }
                    },
                    Dates = new List<Date>
                    {
                        new Date { DateType = "Birth", DateValue = new DateTime(1988, 12, 15) }
                    },
                    Deceased = false,
                    Gender = "Male",
                    Names = new List<Name>
                    {
                        new Name { FirstName = "Michael",MiddleName = "Mason", Surname = "Johnson" }
                    }
                },
                new Entity
                {
                    Id = "4",
                    Addresses = new List<Address>
                    {
                        new Address { AddressLine = "101 Pine St", City = "Mountainview", Country = "USA" }
                    },
                    Dates = new List<Date>
                    {
                        new Date { DateType = "Birth", DateValue = new DateTime(1995, 8, 20) }
                    },
                    Deceased = true,
                    Gender = "Female",
                    Names = new List<Name>
                    {
                        new Name { FirstName = "Emily",MiddleName = "William", Surname = "Brown" }
                    }
                },
                new Entity
                {
                    Id = "5",
                    Addresses = new List<Address>
                    {
                        new Address { AddressLine = "222 Maple St", City = "Largetown", Country = "Canada" }
                    },
                    Dates = new List<Date>
                    {
                        new Date { DateType = "Birth", DateValue = new DateTime(1980, 3, 25) }
                    },
                    Deceased = false,
                    Gender = "Male",
                    Names = new List<Name>
                    {
                        new Name { FirstName = "William" ,MiddleName = "Benjamin", Surname = "Taylor" }
                    }
                },
                new Entity
                {
                    Id = "6",
                    Addresses = new List<Address>
                    {
                        new Address { AddressLine = "555 Cedar St", City = "Ruraltown", Country = "USA" }
                    },
                    Dates = new List<Date>
                    {
                        new Date { DateType = "Birth", DateValue = new DateTime(1992, 6, 8) }
                    },
                    Deceased = false,
                    Gender = "Female",
                    Names = new List<Name>
                    {
                        new Name { FirstName = "Sarah" ,MiddleName = "Jacob", Surname = "Lee" }
                    }
                }

            };

        // POST: api/demo*
        [HttpPost("create")]
        public async Task<ActionResult<Entity>> CreateEntity(Entity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }
            _entities.Add(entity);
            return CreatedAtAction(nameof(GetEntity), new { id = entity.Id }, entity);
        }


        // GET api/entities
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Entity>>> GetEntities()
        {
            return Ok(_entities);
        }


        // GET api/entities/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Entity>> GetEntity(string id)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                return NotFound("Data Not Found");
            }
            return Ok(entity);
        }

        // PUT: api/entity/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Entity>> UpdateEntity(string id, Entity updatedEntity)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            // Update the properties of the entity
            entity.Addresses = updatedEntity.Addresses;
            entity.Dates = updatedEntity.Dates;
            entity.Deceased = updatedEntity.Deceased;
            entity.Gender = updatedEntity.Gender;
            entity.Names = updatedEntity.Names;

            return NoContent();
        }

        // DELETE: api/entity/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEntity(string id)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            _entities.Remove(entity);
            return NoContent();
        }

        // GET api/entities/search
        [HttpGet("search")]
        public ActionResult<IEnumerable<Entity>> SearchEntities([FromQuery] string searchText)
        {
            var results = _entities.Where(e =>
                e.Names.Any(n => n.FirstName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true) ||
                e.Names.Any(n => n.MiddleName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true) ||
                e.Names.Any(n => n.Surname?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true) ||
                e.Addresses.Any(a => a.AddressLine?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true) ||
                e.Addresses.Any(a => a.Country?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true)
            );

            return Ok(results);
        }


        // GET api/entities/filter
        [HttpGet("filter")]
        public ActionResult<IEnumerable<Entity>> FilterEntities(
            [FromQuery] string gender,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string[] countries
            )
        {
            var results = _entities.AsQueryable();

            if (!string.IsNullOrEmpty(gender))
            {
                results = results.Where(e => e.Gender == gender);
            }

            if (startDate.HasValue)
            {
                results = results.Where(e => e.Dates.Any(d => d.DateType == "Birth" && d.DateValue >= startDate));
            }

            if (endDate.HasValue)
            {
                results = results.Where(e => e.Dates.Any(d => d.DateType == "Birth" && d.DateValue <= endDate));
            }

            if (countries != null && countries.Any())
            {
                results = results.Where(e => e.Addresses.Any(a => countries.Contains(a.Country)));
            }

            return Ok(results.ToList());
        }
    }
}
