using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UnCRM.Api.Contract;

namespace UnCRM.Api.Controllers
{
    public class BaseController : Controller
    {
        protected long _idUsuario;
        protected long ObterIdUsuarioLogado()
        {
            var idClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim is null || !long.TryParse(idClaim.Value, out var idUsuario))
                throw new UnauthorizedAccessException("Usuário não autenticado ou ID inválido.");

            return idUsuario;
        }

        protected ModelErrorContract RetornarModelNotFound(Exception ex)
        {
            return new ModelErrorContract
            {
                Status = 404,
                Title = "Not Found",
                Message = ex.Message,
                DateTime = DateTime.Now
            };
        }

        protected ModelErrorContract RetornarModelUnauthorized(Exception ex)
        {
            return new ModelErrorContract
            {
                Status = 401,
                Title = "Unauthorized",
                Message = ex.Message,
                DateTime = DateTime.Now
            };
        }

        protected ModelErrorContract RetornarModelBadRequest(Exception ex)
        {
            return new ModelErrorContract
            {
                Status = 400,
                Title = "Bad Request",
                Message = ex.Message,
                DateTime = DateTime.Now
            };
        }
    }
}