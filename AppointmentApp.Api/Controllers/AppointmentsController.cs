using AppointmentApp.Application.Commands.Appointments;
using AppointmentApp.Application.Queries;
using AppointmentApp.Application.Requests.Appointments;
using AppointmentApp.Application.Responses.Appointments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            try
            {
                var command = new CreateAppointmentCommand(request);
                var result = await _mediator.Send(command);
                return Ok(new
                {
                    message = "Appointment created successfully.",
                    Content = result
                });
            } catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAllAppointmentResponse>>> GetAllAppointments([FromQuery]long agencyId)
        {
            var query = new GetAllAppointmentQuery(agencyId);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPut("{token}/continue")]
        public async Task<IActionResult> UpdateAppointment(string token)
        {
            var command = new UpdateAppointmentCommand(token);
            await _mediator.Send(command);
            return Ok(new { message = "Appointment updated successfully." });
        }

        [HttpGet("queue")]
        public async Task<ActionResult<List<GetOutstandingAppointmentResponse>>> GetOutstandingAppointments(
            [FromQuery] GetOutstandingAppointmentRequest request)
        {
            var query = new GetOutstandingAppointmentQuery(request);

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
