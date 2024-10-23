﻿using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Requests;

public class EmployeeRegisterRequest : EmployeeLoginRequest
{
    [Required(ErrorMessage = "RoleId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "RoleId must be a positive number.")]
    public int RoleId { get; set; }
}