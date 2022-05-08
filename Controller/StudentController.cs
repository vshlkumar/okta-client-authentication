using DemoApplication.Model;
using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StudentController : ControllerBase
    {
    
        [HttpGet("Get Students")]
        public IActionResult GetStudents()
        {
            return Ok(new List<string>
            {
                "Student1",
                "Student2"
            });
        }        
        
        [HttpPost("Create or update student")]
        public IActionResult CreateOrUpdateStudent([FromBody] Student student)
        {
            try
            {
                throw new ArgumentNullException();
            
            //here we perform the logic to create or update student
            var response = new { Name = $"{student.FirstName} {student.LastName}", Email = student.Email };
            return Ok(response);
            }
            catch(Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }
    }
}
