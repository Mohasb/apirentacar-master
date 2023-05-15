using WebAPITest.Models;
using Microsoft.AspNetCore.Mvc;
namespace WebAPITest.Interfaces
{
    public interface IPlanningService
    {
        Task<ActionResult<IEnumerable<Planning>>> GetAllPlanning();
        Task<ActionResult<IEnumerable<PlanningResponse>>> GetAvailableByBranchAndDates(string sucursalId, DateTime fechaInicio, DateTime fechaFinal);
        void SumarDisponibleNuevoCoche(string sucursal, string groupName);
        void RestarDisponibleNuevaReservaMismaSucursal(string sucursal, string groupName, DateTime fechaInicio, DateTime fechaFinal);
        void RestarDisponibleNuevaReservaDiferenteSucursal(string sucursalInicio, string sucursalFinal, string groupName, DateTime fechaInicio, DateTime fechaFinal);
        void SumarDisponibleCanceladoMismaSucursal(string sucursal, string groupName, DateTime fechaInicio, DateTime fechaFinal);
        void SumarDisponibleCanceladoDiferenteSucursal(string sucursalInicio, string sucursalFinal, string groupName, DateTime fechaInicio, DateTime fechaFinal);
        void AddAvailable(Planning planning);
        void AddNewGroup(string group);
        void AddNewBranch(string branch);
    }
}