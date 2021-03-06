﻿using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for adding a new position.
    /// </summary>
    [AutoInject]
    public interface ICreatePositionCommand
    {
        /// <summary>
        ///  Adds a new position. Returns its Id if it succeeded to add a position, otherwise Exception.
        /// </summary>
        /// <param name="request">Position data.</param>
        /// <returns>New position Id.</returns>
        /// <exception cref="ValidationException">Thrown when position data is incorrect.</exception>
        OperationResultResponse<Guid> Execute(CreatePositionRequest request);
    }
}