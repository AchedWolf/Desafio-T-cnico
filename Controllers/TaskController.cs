using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Luby.Data;
using Task = Luby.Models.Task;
using System.Linq;

namespace Luby.Controllers
{
    [ApiController]
    [Route("task")]
    public class TaskController : ControllerBase
    {
        // Registrar nova Task
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Task>> Post(
            [FromServices] DataContext context,
            [FromBody]Task model)
        {
            if(ModelState.IsValid)
            {
                // Validação se usuario existe
                if(!(await context.Users.AnyAsync(x => x.Id == model.UserId)))
                    return BadRequest(new {message = "O usuario que você quer vincular não existe."});

                // Verificando se o usuario mandou um id
                if (model.Id != 0)
                    return BadRequest(new { message = "Não hà necessidade de se mandar o id, pois ele é gerado automaticamente." });

                // Criando Task
                context.Tasks.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Visualizar todas as Tasks
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Task>>> Get([FromServices] DataContext context)
        {
            // Gerado lista de Tasks
            var tasks = await context.Tasks.ToListAsync();
            return tasks;
        }

        // Visualizar uma task pelo Id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Task>> GetById(
            [FromServices] DataContext context, 
            int id)
        {
            // Buscando Task
            var task = await context.Tasks
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);
            return task;
        }

        // Função de Deletar 
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Task>> Delete(
            [FromServices] DataContext context,
            int id)
        {
            // Verificando existencia da Task
            if (!(await context.Tasks.AnyAsync(x => x.Id == id)))
            {
                return NotFound(new {message = "A Task não existe ou já foi apagada."});
            }


            // Deletando Task
            var task = await context.Tasks.FirstAsync(x => x.Id == id);
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();

            return task;
        }

        // Função de Editar
        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Task>> Put(
            [FromServices] DataContext context, 
            int id, 
            [FromBody] Task model)
        {
            // Validação Task e Model
            if(!(await context.Tasks.AnyAsync(x => x.Id == id)) || model == null)
            {
                return NotFound(new {message = "Task ou dados inválidos."});
            }

            // Buscando Task
            var task = await context.Tasks
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            // Verificando e substituindo diferenças
            if(model.description != task.description)
            {
                task.description = model.description;
            }

            if(model.concluded != task.concluded)
            {
                task.concluded = model.concluded;
            }

            if(model.UserId > 0)
            {
                // Validando Id de Usuário
                try
                {
                    var user = await context.Users.FirstAsync(x => x.Id == model.UserId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest(new {message = "Usuário não existe."});
                }

                task.UserId = model.UserId;
            }

            // Update na Task modificada
            context.Tasks.Update(task);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!context.Tasks.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return task;
        }

        // Função de Editar concluded
        [HttpPut]
        [Route("concluded/{id:int}")]
        public async Task<ActionResult<Task>> PutConcluded(
            [FromServices] DataContext context, 
            int id)
        {
            var task = await context.Tasks
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            if(task == null)
            {
                return NotFound();
            }

            task.concluded = true;
            
            context.Tasks.Update(task);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!context.Tasks.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
    }
}