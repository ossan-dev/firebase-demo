using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firebase_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetUserByIdAsync(string uid)
        {
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                return Ok(userRecord);
            }
            catch (FirebaseAuthException authExc)
            {
                return NotFound(authExc.Message);
            }
            catch (FirebaseException fireExc)
            {

                return StatusCode(500, fireExc);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(UserRecordArgs userInfo)
        {
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userInfo);
                return Ok(userRecord);
            }
            catch (FirebaseException fireExc)
            {

                return StatusCode(500, fireExc);
            }
        }
    }
}
