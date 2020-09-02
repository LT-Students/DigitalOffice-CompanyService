using System;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Database.Entities;

namespace LT.DigitalOffice.CompanyService.Repositories.Interfaces
{
    /// <summary>
    /// Represents interface of repository in repository pattern.
    /// Provides methods for working with the database of CompanyService.
    /// </summary>
    public interface IPositionRepository
    {
        /// <summary>
        /// Returns the position with the specified id from database.
        /// </summary>
        /// <param name="positionId">Specified id of position.</param>
        /// <returns>Position with specified id.</returns>
        DbPosition GetPositionById(Guid positionId);

        /// <summary>
        /// Returns a list of all added positions to the database.
        /// </summary>
        /// <returns>List of all added positions.</returns>
        List<DbPosition> GetPositionsList();

        /// <summary>
        /// Returns the position of user.
        /// </summary>
        /// <param name="userId">Specified id of user.</param>
        /// <returns>User's position.</returns>
        DbPosition GetUserPosition(Guid userId);

        /// <summary>
        /// Disable the position with the specified id from database.
        /// </summary>
        /// <param name="positionId">Specified id of position.</param>
        /// <returns>Nothing if the position was disabled, otherwise Exception.</returns>
        void DisablePositionById(Guid positionId);

        /// <summary>
        /// Adds new position to the database. Returns its Id.
        /// </summary>
        /// <param name="position">Position to add.</param>
        /// <returns>New position Id.</returns>
        Guid AddPosition(DbPosition position);

        /// <summary>
        /// Edits an existing position in the database. Returns whether it was successful to edit.
        /// </summary>
        /// <param name="position">Position to edit.</param>
        /// <returns>Whether it was successful to edit.</returns>
        bool EditPosition(DbPosition position);
    }
}