using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyDemo.Common.Helpers;
using TinyDemo.Common.Models;
using TinyDemo.Services;

namespace TinyDemo.Controllers
{
    [Route("v1/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string TokenCookieName = "Tiny.Access";
        private readonly AuthenticationService service;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            service = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult> AuthenticateAsync([FromBody] AuthRequestViewModel model)
        {
            try
            {
                var response = await service.AuthenticateAsync(model, IpAddress());

                SetTokenCookie(response.RefreshToken);

                return Ok(response);

            }
            catch (IncorrectCredendentialsException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshAsync()
        {
            var refreshToken = Request.Cookies[TokenCookieName];

            if(string.IsNullOrEmpty(refreshToken))
            {
                return NoContent();
            }

            try
            {
                
                var response = await service.RefreshTokenAsync(refreshToken, IpAddress());

                SetTokenCookie(response.RefreshToken);

                return Ok(response);

            }
            catch (InvalidTokenException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("revoke")]
        public ActionResult Revoke([FromBody] AuthRevokeViewModel model)
        {
            try
            {
                // accept token from request body or cookie
                var token = model.Token ?? Request.Cookies[TokenCookieName];
                var response = service.RevokeToken(token, IpAddress());

                return Ok(new { message = "Token revoked" });

            }
            catch (InvalidTokenException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // TODO Implement Change password

        // TODO Implement Change email

        #region helpers

        private void SetTokenCookie(string token)
        {
            // TODO To be removed later to make it independent of UI
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append(TokenCookieName, token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        #endregion
    }

}
