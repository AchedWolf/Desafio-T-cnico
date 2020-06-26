using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Luby.Data;
using Luby.Models;
using Luby.Services;

namespace Luby.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        // Registro
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
            [FromServices] DataContext context,
            [FromBody]User model)
        {
            if(ModelState.IsValid)
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Autenticação
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromBody]User model,
            [FromServices] DataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync(
                x => x.user == model.user && 
                x.password == model.password);
            
            if(user == null)
            {
                return NotFound(new {message = "Usuário ou senha inválidos."});
            }

            var token = TokenService.GenerateToken(user);
            user.password = "";

            return new
            {
                user = user,
                token = token
            };
        }
    }
}