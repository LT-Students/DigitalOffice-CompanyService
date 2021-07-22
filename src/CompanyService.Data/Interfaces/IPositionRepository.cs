using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    /// <summary>
    /// Represents the repository pattern.
    /// Provides methods for working with the database of CompanyService.
    /// </summary>
    [AutoInject]
    public interface IPositionRepository
    {
        /// <summary>
        /// Returns the position with the specified id from database.
        /// </summary>
        /// <param name="positionId">Specified id of position.</param>
        /// <returns>Position with specified id.</returns>
        DbPosition Get(Guid? positionId, Guid? userId);

        /// <summary>
        /// Returns a list of all added positions to the database.
        /// </summary>
        /// <returns>List of all added positions.</returns>
        List<DbPosition> Find(int skipCount, int takeCount, bool includeDeactivated, out int totalCount);

        /// <summary>
        /// Disable the position with the specified id from database.
        /// </summary>
        /// <param name="positionId">Specified id of position.</param>
        /// <returns>Nothing if the position was disabled, otherwise Exception.</returns>
        bool PositionContainsUsers(Guid positionId);

        /// <summary>
        /// Adds new position to the database. Returns its Id.
        /// </summary>
        /// <param name="position">Position to add.</param>
        /// <returns>New position Id.</returns>
        Guid Create(DbPosition position);

        /// <summary>
        /// Edits an existing position in the database. Returns whether it was successful to edit
        /// </summary>
        /// <param name="positionId">Id of edited position.</param>
        /// <param name="request">Edit request.</param>
        /// <returns>Whether it was successful to edit.</returns>
        bool Edit(Guid positionId, JsonPatchDocument<DbPosition> request);

        bool DoesNameExist(string name);

        bool Contains(Guid positionId);
    }
}