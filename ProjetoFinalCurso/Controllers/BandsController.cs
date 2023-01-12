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
    

    public class BandsController : Controller
    {
        private readonly IBandRepository _bandRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IFlashMessage _flashMessage;

        public BandsController(IBandRepository bandRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper
            , IFlashMessage flashMessage)
        {
            _bandRepository = bandRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _flashMessage = flashMessage;
        }

        // GET: Bands
        public IActionResult Index()
        {
            return View(_bandRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Bands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BandNotFound");
            }

            var model = await _bandRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("BandNotFound");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: Bands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BandViewModel band)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (band.ImageFile != null && band.ImageFile.Length > 0)
                {


                    imageId = await _blobHelper.UploadBlobAsync(band.ImageFile, "band");

                }


                var product = _converterHelper.toBand(band, imageId, true);




                product.User = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);


                try
                {
                    await _bandRepository.CreateAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _flashMessage.Danger("This band already exist!");
                }


            }
            return View(band);
        }
        [Authorize(Roles = "Admin")]
        // GET: Bands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BandNotFound");
            }

            var model = await _bandRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("BandNotFound");
            }
            var viewmodel = _converterHelper.toBandViewModel(model);

            return View(viewmodel);
        }

        // POST: Bands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BandViewModel band)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (band.ImageFile != null && band.ImageFile.Length > 0)
                    {


                        imageId = await _blobHelper.UploadBlobAsync(band.ImageFile, "bands");

                    }


                    var product = _converterHelper.toBand(band, imageId, false);


                    product.User = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);
                    await _bandRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bandRepository.ExistAsync(band.Id))
                    {
                        return new NotFoundViewResult("EstablishmentNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(band);
        }
        [Authorize(Roles = "Admin")]
        // GET: Bands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BandNotFound");
            }

            var model = await _bandRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("BandNotFound");
            }

            return View(model);
        }

        // POST: Bands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _bandRepository.GetByIdAsync(id);

            try
            {
                await _bandRepository.DeleteAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{product.Name} is probably being used!!";
                    ViewBag.ErrorMessage = $"{product.Name} can´t be delete since its being used in a concert.</br></br>" +
                       $"First delete the concerts that are using it then try again.";
                }




                return View("Error");

            }
        }

        public IActionResult BandNotFound()
        {
            return View();
        }
    }
}
