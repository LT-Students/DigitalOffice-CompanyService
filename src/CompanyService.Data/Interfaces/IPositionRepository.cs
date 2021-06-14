using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
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
        DbPosition GetPosition(Guid? positionId, Guid? userId);

        /// <summary>
        /// Returns a list of all added positions to the database.
        /// </summary>
        /// <returns>List of all added positions.</returns>
        List<DbPosition> FindPositions();

        /// <summary>
        /// Disable the position with the specified id from database.
        /// </summary>
        /// <param name="positionId">Specified id of position.</param>
        /// <returns>Nothing if the position was disabled, otherwise Exception.</returns>
        void DisablePosition(Guid positionId);

        /// <summary>
        /// Adds new position to the database. Returns its Id.
        /// </summary>
        /// <param name="position">Position to add.</param>
        /// <returns>New position Id.</returns>
        Guid CreatePosition(DbPosition position);

        /// <summary>
        /// Edits an existing position in the database. Returns whether it was successful to edit.
        /// </summary>
        /// <param name="position">Position to edit.</param>
        /// <returns>Whether it was successful to edit.</returns>
        bool EditPosition(DbPosition position);
    }
}