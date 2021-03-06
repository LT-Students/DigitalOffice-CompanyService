﻿using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Validation.Helper;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Validation.Department
{
    public class EditDepartmentRequestValidator : BaseEditRequestValidator<EditDepartmentRequest>, IEditDepartmentRequestValidator
    {
        private void HandleInternalPropertyValidation(Operation<EditDepartmentRequest> requestedOperation, CustomContext context)
        {
            RequestedOperation = requestedOperation;
            Context = context;

            #region Paths

            AddСorrectPaths(
                new List<string>
                {
                    nameof(EditDepartmentRequest.Name),
                    nameof(EditDepartmentRequest.Description),
                    nameof(EditDepartmentRequest.IsActive)
                });

            AddСorrectOperations(nameof(EditDepartmentRequest.Name), new() { OperationType.Replace });
            AddСorrectOperations(nameof(EditDepartmentRequest.Description), new() 
            { 
                OperationType.Add, OperationType.Replace, OperationType.Remove 
            });
            AddСorrectOperations(nameof(EditDepartmentRequest.IsActive), new() { OperationType.Replace });

            #endregion

            #region Name

            AddFailureForPropertyIf(
                nameof(EditDepartmentRequest.Name),
                x => x == OperationType.Replace,
                new()
                {
                    { x => !string.IsNullOrEmpty(x.value.ToString()), "Name is too short." },
                });

            #endregion

            #region Description

            AddFailureForPropertyIf(
                nameof(EditDepartmentRequest.Description),
                x => x == OperationType.Replace || x == OperationType.Add,
                new()
                {
                    { x => !string.IsNullOrEmpty(x.value.ToString()), "Description is too short." },
                });

            #endregion

            #region DirectorId

            AddFailureForPropertyIf(
                nameof(EditDepartmentRequest.DirectorId),
                x => x == OperationType.Replace,
                new()
                {
                    { x => Guid.TryParse(x.value.ToString(), out Guid _), "Incorrect format of DirectorId." },
                });

            #endregion

            #region IsActive

            AddFailureForPropertyIf(
                nameof(EditDepartmentRequest.IsActive),
                x => x == OperationType.Replace,
                new()
                {
                    { x => bool.TryParse(x.value.ToString(), out bool _), "Incorrect format of IsActive." },
                });

            #endregion
        }

        public EditDepartmentRequestValidator()
        {
            RuleForEach(x => x.Operations)
               .Custom(HandleInternalPropertyValidation);
        }
    }
}
