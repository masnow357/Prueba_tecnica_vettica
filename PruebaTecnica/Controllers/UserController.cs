using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PruebaTecnica.Models;
using PruebaTecnica.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace PruebaTecnica.Controllers;

[Route("api")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfigurationRoot Configuration;
    public UserController()
	{

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json");

        Configuration = configurationBuilder.Build();
        
    }

    [HttpPost("createUser")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<IBasicResponse<User>>> CreateUser(User user)
    {
        string url = "https://osgqhdx2wf.execute-api.us-west-2.amazonaws.com/card/valid/" + user.CardSerial;
        string bearerToken = Configuration.GetSection("BalanceAndInformationToken").Value;

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var cardValidationResponse = JsonSerializer.Deserialize<CardValidationResponse>(responseBody);
                if (!cardValidationResponse.isValid)
                {
                    IBasicResponse<User> basicResponse = new BasicResponse<User>()
                    {
                        Message = "Invalid Card",
                        Date = DateTime.Now,
                        Data = null
                    };
                    return BadRequest(basicResponse);
                }
            }
            else
            {
                IBasicResponse<User> basicResponse = new BasicResponse<User>()
                {
                    Message = "Invalid Card",
                    Date = DateTime.Now,
                    Data = null
                };
                return BadRequest(basicResponse);
            }
        }

        var modelsContext = new ModelsContext(Configuration);
        user.RoleID = 2;
        modelsContext.Add(user);
        modelsContext.SaveChanges();

        IBasicResponse<User> data = new BasicResponse<User>()
        {
            Message = "Success",
            Date = DateTime.Now,
            Data = user
        };

        return Ok(data);

	}

    [HttpGet("getUser/{id}")]
    [Authorize(Roles = "admin")]
    public ActionResult<IBasicResponse<User>> GetUser(int id)
    {

        using (var modelsContext = new ModelsContext(Configuration))
        {
            var user = modelsContext.Users.Find(id);

            if (user == null)
            {
                IBasicResponse<User> basicResponse = new BasicResponse<User>()
                {
                    Message = "User not found",
                    Date = DateTime.Now,
                    Data = null
                };
                return NotFound(basicResponse);
            }

            IBasicResponse<User> data = new BasicResponse<User>()
            {
                Message = "Success",
                Date = DateTime.Now,
                Data = user
            };

            return Ok(data);
        }
    }

    [HttpPut("updateUser/{id}")]
    [Authorize(Roles = "admin")]

    public async Task<ActionResult<IBasicResponse<User>>> UpdateUser(int id, User updatedUser)
    {
        // Find the existing user in the database
        var modelsContext = new ModelsContext(Configuration);
        var user = modelsContext.Users.Find(id);

        if (user == null)
        {
            IBasicResponse<User> basicResponse = new BasicResponse<User>()
            {
                Message = "User not found",
                Date = DateTime.Now,
                Data = null
            };
            return NotFound(basicResponse);
        }

        if (updatedUser.CardSerial != user.CardSerial)
        {
            string url = "https://osgqhdx2wf.execute-api.us-west-2.amazonaws.com/card/valid/" + updatedUser.CardSerial;
            string bearerToken = Configuration.GetSection("BalanceAndInformationToken").Value;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var cardValidationResponse = JsonSerializer.Deserialize<CardValidationResponse>(responseBody);
                    if (!cardValidationResponse.isValid)
                    {
                        IBasicResponse<User> basicResponse = new BasicResponse<User>()
                        {
                            Message = "Invalid Card",
                            Date = DateTime.Now,
                            Data = null
                        };
                        return BadRequest(basicResponse);
                    }
                }
                else
                {
                    IBasicResponse<User> basicResponse = new BasicResponse<User>()
                    {
                        Message = "Invalid Card",
                        Date = DateTime.Now,
                        Data = null
                    };
                    return BadRequest(basicResponse);
                }
            }
        }

        // Update the properties of the user
        user.CardSerial = updatedUser.CardSerial;
        user.Email = updatedUser.Email;

        // Save the changes to the database
        modelsContext.SaveChanges();

        IBasicResponse<User> data = new BasicResponse<User>()
        {
            Message = "Success",
            Date = DateTime.Now,
            Data = user
        };

        return Ok(user);
    }

    [HttpDelete("deleteUser/{id}")]
    [Authorize(Roles = "admin")]

    public ActionResult DeleteUser(int id)
    {
        // Find the existing user in the database
        using (var modelsContext = new ModelsContext(Configuration))
        {
            var user = modelsContext.Users.Find(id);
            if (user == null)
            {
                IBasicResponse<User> basicResponse = new BasicResponse<User>()
                {
                    Message = "User not found",
                    Date = DateTime.Now,
                    Data = null
                };
                return NotFound(basicResponse);
            }

            // Remove the user from the DbContext
            modelsContext.Users.Remove(user);

            // Save the changes to the database
            modelsContext.SaveChanges();

            IBasicResponse<User> data = new BasicResponse<User>()
            {
                Message = "Success",
                Date = DateTime.Now,
                Data = user
            };

            return Ok(data);
        }
    }
}

