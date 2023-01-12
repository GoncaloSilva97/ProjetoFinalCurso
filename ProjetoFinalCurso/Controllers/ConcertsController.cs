using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalCurso.Data;
using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Data.Repository;
using ProjetoFinalCurso.Helpers;
using ProjetoFinalCurso.Models;
using Vereyon.Web;

namespace ProjetoFinalCurso.Controllers
{
    public class ConcertsController : Controller
    {
        
        private readonly IConcertRepository _concertRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IFlashMessage _flashMessage;

        public ConcertsController(IConcertRepository concertRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper
            , IFlashMessage flashMessage)
        {
            _concertRepository = concertRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _flashMessage = flashMessage;
        }

        // GET: Concerts
        public async  Task<IActionResult> Index()
        {
            if (this.User.Identity.Name == null)
            {
                return View(_concertRepository.GetConcertsSimple());
            }

            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

            bool b = false;

            if (this.User.IsInRole("Admin") || this.User.IsInRole("Customer"))
            {
                b = true;
            }

            return View(_concertRepository.GetConcertsWithBands(b, user));
        }

        // GET: Concerts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            var model = await _concertRepository.GetByIdConcertsWithBandsAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            return View(model);
        }
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Bands(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            var country = await _concertRepository.GetByIdConcertsWithBandsAsync(id.Value);
            if (country == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            return View(country);
        }
        [Authorize(Roles = "Admin, Employee")]
        // GET: Concerts/Create
        public async Task<IActionResult> Create()
        {

            var viewmodel = new ConcertViewModel();

            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

            viewmodel.ListEstablishments = _concertRepository.GetComboEstablishments(this.User.IsInRole("Admin"), user);

            return View(viewmodel);
        }

        // POST: Concerts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConcertViewModel concert)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (concert.ImageFile != null && concert.ImageFile.Length > 0)
                {


                    imageId = await _blobHelper.UploadBlobAsync(concert.ImageFile, "concerts");

                }
                

                var product = _converterHelper.toConcert(concert, imageId, true);




                product.User = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

                try
                {
                    await _concertRepository.CreateAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    //_flashMessage.Danger("This country already exist!");
                }

                return View(concert);
            }
            return View(concert);
        }
        [Authorize(Roles = "Admin, Employee")]
        // GET: Concerts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            var model = await _concertRepository.GetByIdConcertsWithBandsAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }
            var viewmodel = _converterHelper.toConcertViewModel(model);

            

            viewmodel.ListEstablishments = _concertRepository.GetComboEstablishments(this.User.IsInRole("Admin"), viewmodel.User);

            return View(viewmodel);
        }

        // POST: Concerts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ConcertViewModel concert)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (concert.ImageFile != null && concert.ImageFile.Length > 0)
                    {


                        imageId = await _blobHelper.UploadBlobAsync(concert.ImageFile, "concerts");

                    }


                    var product = _converterHelper.toConcert(concert, imageId, false);


                    product.User = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);
                    await _concertRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _concertRepository.ExistAsync(concert.Id))
                    {
                        return new NotFoundViewResult("ConcertNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(concert);
        }
        [Authorize(Roles = "Admin, Employee")]
        // GET: Concerts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            var model = await _concertRepository.GetByIdConcertsWithBandsAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            return View(model);

        }

        // POST: Concerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _concertRepository.GetByIdAsync(id);

            try
            {
                await _concertRepository.DeleteAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{product.Title} is probably being used!!";
                    ViewBag.ErrorMessage = $"{product.Title} can´t be delete since its being used in a ticket.</br></br>" +
                       $"First delete the tickets that are using it then try again.";
                }

                return View("Error");

            }
        }
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> AddBand(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            var country = await _concertRepository.GetByIdConcertsWithBandsAsync(id.Value);
            if (country == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            

            var model = new ConcertBandViewModel { ConcertId = country.Id };

            model.ListBands = _concertRepository.GetComboBands(country);

            return View(model);
        }
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        public async Task<IActionResult> AddBand(ConcertBandViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                model = await _concertRepository.AddBandAsync(model);
                return RedirectToAction("Bands", new { id = model.ConcertId });
            }

            return this.View(model);
        }
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> RemoveBand(int? id/*, int? idConcert*/)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }


            var band = await _concertRepository.GetBandAsync(id.Value);
            if (band == null)
            {
                return new NotFoundViewResult("ConcertNotFound");
            }

            await _concertRepository.RemoveBandAsync(band);

            return RedirectToAction("Index");
        }

        public IActionResult ConcertNotFound()
        {
            return View();
        }
    }
}
