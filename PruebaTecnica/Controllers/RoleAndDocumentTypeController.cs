using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PruebaTecnica.Interfaces;
using PruebaTecnica.Models;
using System.Data;
using System.Text.Json;

namespace PruebaTecnica.Controllers
{
    [Route("api")]
    [ApiController]
    public class RoleAndDocumentTypeController : ControllerBase
    {
        private readonly IConfigurationRoot Configuration;

        public RoleAndDocumentTypeController()
        {

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");

            Configuration = configurationBuilder.Build();
        }

        [HttpPost("CreateRole")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<IBasicResponse<Role>>> CreateRole(Role role)
        {
            var modelsContext = new ModelsContext(Configuration);

            modelsContext.Add(role);
            modelsContext.SaveChanges();

            IBasicResponse<Role> data = new BasicResponse<Role>()
            {
                Message = "Success",
                Date = DateTime.Now,
                Data = role
            };

            return Ok(data);

        }

        [HttpDelete("deleteRole/{id}")]
        [Authorize(Roles = "admin")]

        public ActionResult DeleteRole(int id)
        {
            // Find the existing user in the database
            using (var modelsContext = new ModelsContext(Configuration))
            {
                var role = modelsContext.Role.Find(id);
                if (role == null)
                {
                    IBasicResponse<Role> basicResponse = new BasicResponse<Role>()
                    {
                        Message = "Role not found",
                        Date = DateTime.Now,
                        Data = null
                    };
                    return NotFound(basicResponse);
                }

                // Remove the user from the DbContext
                modelsContext.Role.Remove(role);

                // Save the changes to the database
                modelsContext.SaveChanges();

                IBasicResponse<Role> data = new BasicResponse<Role>()
                {
                    Message = "Success",
                    Date = DateTime.Now,
                    Data = role
                };

                return Ok(data);
            }
        }


        // This section is for DocumentType


        [HttpPost("CreateDocumentType")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<IBasicResponse<DocumentType>>> CreateDocumentType(DocumentType documentType)
        {
            var modelsContext = new ModelsContext(Configuration);

            modelsContext.Add(documentType);
            modelsContext.SaveChanges();

            IBasicResponse<DocumentType> data = new BasicResponse<DocumentType>()
            {
                Message = "Success",
                Date = DateTime.Now,
                Data = documentType
            };

            return Ok(data);

        }

        [HttpDelete("deleteDocumentType/{id}")]
        [Authorize(Roles = "admin")]

        public ActionResult DeleteDocumentType(int id)
        {
            // Find the existing user in the database
            using (var modelsContext = new ModelsContext(Configuration))
            {
                var documentType = modelsContext.DocumentType.Find(id);
                if (documentType == null)
                {
                    IBasicResponse<DocumentType> basicResponse = new BasicResponse<DocumentType>()
                    {
                        Message = "DocumentType not found",
                        Date = DateTime.Now,
                        Data = null
                    };
                    return NotFound(basicResponse);
                }

                // Remove the user from the DbContext
                modelsContext.DocumentType.Remove(documentType);

                // Save the changes to the database
                modelsContext.SaveChanges();

                IBasicResponse<DocumentType> data = new BasicResponse<DocumentType>()
                {
                    Message = "Success",
                    Date = DateTime.Now,
                    Data = documentType
                };

                return Ok(data);
            }
        }


    }
}
