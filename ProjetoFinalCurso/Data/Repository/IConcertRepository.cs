using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalCurso.Data.Repository
{
    public interface IConcertRepository : IGenericRepository<Concert>
    {
        IQueryable GetConcertsSimple();

        IQueryable GetConcertsWithBands(bool b, User user);

        Task<Concert> GetByIdConcertsWithBandsAsync(int id);

        Task<Band> GetBandAsync(int id);

        Task<ConcertBandViewModel> AddBandAsync(ConcertBandViewModel model);

        Task<int> RemoveBandAsync(Band band);

        IEnumerable<SelectListItem> GetComboBands(Concert concert);

        IEnumerable<SelectListItem> GetComboEstablishments(bool b,User user);



        //Task<Concert> GetConcertAsync(Band band);

        //Task<int> UpdateCityAsync(City city);



        //IEnumerable<SelectListItem> GetComboConcerts();




    }
}
