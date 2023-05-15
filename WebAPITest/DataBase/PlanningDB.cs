using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace WebAPITest.DataBase
{
    public class PlanningDB : IPlanningService
    {
        private readonly DataContext _context;
        private readonly ISaveChangesService _saveChanges;
        public PlanningDB(DataContext context, ISaveChangesService saveChanges)
        {
            _context = context;
            _saveChanges = saveChanges;
        }

        public async Task<ActionResult<IEnumerable<Planning>>> GetAllPlanning()
        {
            return await _context.Plannings.ToListAsync();
        }
        public async Task<ActionResult<IEnumerable<PlanningResponse>>> GetAvailableByBranchAndDates(string sucursal, DateTime fechaInicio, DateTime fechaFinal)
        {
            var listaNoDisponibles = await _context.Plannings.Where(pl => pl.Branch == sucursal
                        && pl.Dia >= fechaInicio
                        && pl.Dia <= fechaFinal
                        && pl.Available == 0).Select(pl => pl.GroupName).Distinct().ToListAsync();

            return await (_context.Plannings.Where( planning => 
                planning.Branch == sucursal && 
                planning.Dia == fechaInicio && 
                !listaNoDisponibles.Contains(planning.GroupName))
            ).Select(planning => 
                new PlanningResponse(planning.GroupName, planning.Available)
            ).Distinct().ToListAsync();
        }
        public void SumarDisponibleNuevoCoche(string sucursal, string groupName)
        {
            var listaDisponibles = _context.Plannings.Where(pl => pl.Branch == sucursal && pl.GroupName == groupName).ToListAsync().Result;
            foreach(Planning disponible in listaDisponibles)
            {
                disponible.Available += 1;
            }
            _saveChanges.SaveChangesDatabase();
        }

        public void RestarDisponibleNuevaReservaMismaSucursal(string sucursal, string groupName, DateTime fechaInicio, DateTime fechaFinal)
        {
            var listaDisponibles = _context.Plannings.Where(pl => pl.Branch == sucursal && pl.GroupName == groupName && pl.Dia >= fechaInicio && pl.Dia <= fechaFinal).ToListAsync().Result;
            Console.WriteLine(listaDisponibles);
            foreach(Planning disponible in listaDisponibles)
            {
                disponible.Available -= 1;
            }
            _saveChanges.SaveChangesDatabase();
        }

        public void RestarDisponibleNuevaReservaDiferenteSucursal(string sucursalInicio, string sucursalFinal, string groupName, DateTime fechaInicio, DateTime fechaFinal)
        {
            var listaDisponiblesSucursalInicio = _context.Plannings.Where(pl => pl.Branch == sucursalInicio && pl.GroupName == groupName && pl.Dia >= fechaInicio).ToListAsync().Result;
            foreach(Planning disponible in listaDisponiblesSucursalInicio)
            {
                disponible.Available -= 1;
            }
            var listaDisponiblesSucursalFinal = _context.Plannings.Where(pl => pl.Branch == sucursalFinal && pl.GroupName == groupName && pl.Dia >= fechaFinal).ToListAsync().Result;
            foreach(Planning disponible in listaDisponiblesSucursalFinal)
            {
                disponible.Available += 1;
            }
            _saveChanges.SaveChangesDatabase();
        }

        public void SumarDisponibleCanceladoMismaSucursal(string sucursal, string groupName, DateTime fechaInicio, DateTime fechaFinal)
        {
            var listaDisponibles = _context.Plannings.Where(pl => pl.Branch == sucursal && pl.GroupName == groupName && pl.Dia >= fechaInicio && pl.Dia < fechaFinal).ToListAsync().Result;
            foreach(Planning disponible in listaDisponibles)
            {
                disponible.Available += 1;
            }
            _saveChanges.SaveChangesDatabase();
        }
        public void SumarDisponibleCanceladoDiferenteSucursal(string sucursalInicio, string sucursalFinal, string groupName, DateTime fechaInicio, DateTime fechaFinal)
        {
            var listaDisponiblesSucursalInicio = _context.Plannings.Where(pl => pl.Branch == sucursalInicio && pl.GroupName == groupName && pl.Dia >= fechaInicio).ToListAsync().Result;
            foreach(Planning disponible in listaDisponiblesSucursalInicio)
            {
                disponible.Available += 1;
            }
            var listaDisponiblesSucursalFinal = _context.Plannings.Where(pl => pl.Branch == sucursalFinal && pl.GroupName == groupName && pl.Dia >= fechaFinal).ToListAsync().Result;
            foreach(Planning disponible in listaDisponiblesSucursalFinal)
            {
                disponible.Available -= 1;
            }
            _saveChanges.SaveChangesDatabase();
        }

        public void AddAvailable(Planning planning)
        {
            _context.Add(planning);
            _saveChanges.SaveChangesDatabase();
        }

        public void AddNewGroup(string group)
        {
            List<DateTime> listaDia = _context.Plannings.Select(planning => planning.Dia).Distinct().ToListAsync().Result;
            List<Branch> listaSucursal = _context.Branches.ToListAsync().Result;

            foreach(DateTime dia in listaDia)
            {
                foreach(Branch sucursal in listaSucursal)
                {
                    AddAvailable(new Planning(dia, sucursal.Name!, group, 0));
                }
            }
        }

        public void AddNewBranch(string branch)
        {
            List<DateTime> listaDia = _context.Plannings.Select(planning => planning.Dia).Distinct().ToListAsync().Result;
            List<Group> listaGrupos = _context.Groups.ToListAsync().Result;

            foreach(DateTime dia in listaDia)
            {
                foreach(Group grupo in listaGrupos)
                {
                    AddAvailable(new Planning(dia, branch, grupo.Name!, 0));
                }
            }
        }
    }
}