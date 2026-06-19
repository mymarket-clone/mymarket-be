using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Users.Commands.Add;
using Mymarket.Application.Features.Users.Commands.Block;
using Mymarket.Application.Features.Users.Commands.Delete;
using Mymarket.Application.Features.Users.Commands.Edit;
using Mymarket.Application.Features.Users.Commands.SetPermissions;
using Mymarket.Application.Features.Users.Commands.SetSuperAdmin;
using Mymarket.Application.Features.Users.Queries.Get;
using Mymarket.Application.Features.Users.Queries.GetAdminById;
using Mymarket.Application.Features.Users.Queries.GetPermissions;
using Mymarket.Domain.Enums;
using Mymarket.Infrastructure.Authentication.Policies;

namespace Mymarket.WebApi.Controllers;

[Route("api/user-management")]
[ApiController]
public class UserManagementController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.UsersView)]
    public async Task<IActionResult> GetUsers([FromQuery] AdminGetUsersQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.UsersView)]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var result = await mediator.Send(new AdminGetUserByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [HasPermission(Permissions.UsersAdd)]
    public async Task<IActionResult> AddUser(AddAdminUserCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { id }, new { id });
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UsersEdit)]
    public async Task<IActionResult> EditUser(
        [FromRoute] int id,
        EditAdminUserCommand command)
    {
        try
        {
            await mediator.Send(command with { Id = id });
        }
        catch (ValidationException ex)
        {
            return BadRequest(CreateToastValidationProblem(ex));
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.UsersDelete)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await mediator.Send(new DeleteAdminUserCommand(id));
        return NoContent();
    }

    [HttpPut("{id}/block")]
    [HasPermission(Permissions.UsersBlock)]
    public async Task<IActionResult> BlockUser(
        [FromRoute] int id,
        BlockUserCommand command)
    {
        await mediator.Send(command with { Id = id });
        return NoContent();
    }

    [HttpPut("{id}/superadmin")]
    [HasPermission(Permissions.UsersEdit, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> SetSuperAdmin(
        [FromRoute] int id,
        SetUserSuperAdminCommand command)
    {
        try
        {
            await mediator.Send(command with { Id = id });
        }
        catch (ValidationException ex)
        {
            return BadRequest(CreateToastValidationProblem(ex));
        }

        return NoContent();
    }

    [HttpGet("{id}/permissions")]
    [HasPermission(Permissions.UsersEdit, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> GetPermissions([FromRoute] int id)
    {
        var result = await mediator.Send(new GetUserPermissionsQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id}/permissions")]
    [HasPermission(Permissions.UsersEdit, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> SetPermissions(
        [FromRoute] int id,
        [FromBody] SetUserPermissionsCommand command)
    {
        try
        {
            await mediator.Send(new SetUserPermissionsCommand(id, command.Permissions));
        }
        catch (ValidationException ex)
        {
            return BadRequest(CreateToastValidationProblem(ex));
        }

        return NoContent();
    }

    private ValidationProblemDetails CreateToastValidationProblem(ValidationException ex)
    {
        var errors = ex.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

        var problem = new ValidationProblemDetails(errors)
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "Validation Failed",
            Status = StatusCodes.Status400BadRequest,
            Instance = $"{Request.Method} {Request.Path}"
        };

        problem.Extensions["code"] = "ValidationError";
        problem.Extensions["message"] = errors
            .SelectMany(x => x.Value)
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

        return problem;
    }
}
