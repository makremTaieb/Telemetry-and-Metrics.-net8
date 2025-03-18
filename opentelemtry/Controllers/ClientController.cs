using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace opentelemtry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;

        public ClientController(ILogger<ClientController> logger)
        {
            _logger = logger;
        }


        // GET: api/<ClientController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("return all Clients");
            return new string[] { "value1", "value2" };
        }

        // GET api/<ClientController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 1)
            {
                _logger.LogWarning($"invalid Client model {ModelState.Count} {id} ", id);
                return BadRequest();
            }
            var options = new RestClientOptions("https://localhost:7101/")
            {
             
            };
            var client = new RestClient(options);
            var request = new RestRequest("api/client", Method.Get);
            

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {

                _logger.LogInformation("return Client id {id}", id);
            }
            return Ok($"Client : {id}");
        }

        // POST api/<ClientController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ClientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
