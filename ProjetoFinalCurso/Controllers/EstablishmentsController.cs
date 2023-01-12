using System;
using System.Collections.Generic;
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
    
    public class EstablishmentsController : Controller
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IFlashMessage _flashMessage;

        public EstablishmentsController(IEstablishmentRepository establishmentRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper
            , IFlashMessage flashMessage)
        {
            _establishmentRepository = establishmentRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _flashMessage = flashMessage;
        }
        //[Authorize(Roles = "Admin, Employee")]
        // GET: Establishments
        public async Task< IActionResult> Index()
        {
            if(this.User.Identity.Name == null)
            {
                return View(_establishmentRepository.GetAllSimple() );
            }

            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

            bool b = false;

            if (this.User.IsInRole("Admin") || this.User.IsInRole("Customer"))
            {
                b = true;
            }

            return View(_establishmentRepository.GetAllWithUsers(b, user));
        }

        // GET: Establishments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("EstablishmentNotFound");
            }

            var model = await _establishmentRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("EstablishmentNotFound");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: Establishments/Create
        public async Task<IActionResult> Create()
        {
            var model = new EstablishmentViewModel();

            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

            model.ListUsers = _establishmentRepository.GetComboUsers(this.User.IsInRole("Admin"), user);

            if (!this.User.IsInRole("Admin"))
            {
                model.UserId = user.Id;
            }

            
            return View(model);
        }

        // POST: Establishments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstablishmentViewModel establishment)
        {
            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

            if (!this.User.IsInRole("Admin"))
            {
                establishment.UserId = user.Id;
            }

            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (establishment.ImageFile != null && establishment.ImageFile.Length > 0)
                {


                    imageId = await _blobHelper.UploadBlobAsync(establishment.ImageFile,"establishments");

                }

               


                var product = _converterHelper.toEstablishment(establishment, imageId, true);


                if (!this.User.IsInRole("Admin"))
                {
                    product.User = await _userHelper.GetUserByIdAsync(establishment.UserId.ToString());
                }
                else
                {
                    product.User = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);
                }
                


                try
                {
                    await _establishmentRepository.CreateAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _flashMessage.Danger("This establishment already exist!");
                }


            }

            return View(establishment);
        }
        [Authorize(Roles = "Admin, Employee")]
        // GET: Establishments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("EstablishmentNotFound");
            }

            var model = await _establishmentRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("EstablishmentNotFound");
            }
            var viewmodel = _converterHelper.toEstablishmentViewModel(model);

            

            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

            viewmodel.ListUsers = _establishmentRepository.GetComboUsers(this.User.IsInRole("Admin"), user);

            if (!this.User.IsInRole("Admin"))
            {
                viewmodel.UserId = user.Id;
            }

            return View(viewmodel);
        }

        [Authorize(Roles = "Admin")]
        // POST: Establishments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EstablishmentViewModel establishment)
        {
            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

            if (!this.User.IsInRole("Admin"))
            {
                establishment.UserId = user.Id;
            }


            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (establishment.ImageFile != null && establishment.ImageFile.Length > 0)
                    {


                        imageId = await _blobHelper.UploadBlobAsync(establishment.ImageFile, "establishments");

                    }


                    var product = _converterHelper.toEstablishment(establishment, imageId, false);


                    if (!this.User.IsInRole("Admin"))
                    {
                        product.User = await _userHelper.GetUserByIdAsync(establishment.UserId.ToString());
                    }
                    else
                    {
                        product.User = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);
                    }


                    await _establishmentRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _establishmentRepository.ExistAsync(establishment.Id))
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
            return View(establishment);
        }
        [Authorize(Roles = "Admin")]
        // GET: Establishments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("EstablishmentNotFound");
            }

            var model = await _establishmentRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("EstablishmentNotFound");
            }

            return View(model);

            
        }

        // POST: Establishments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _establishmentRepository.GetByIdAsync(id);

            try
            {
                await _establishmentRepository.DeleteAsync(product);
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

        public IActionResult EstablishmentNotFound()
        {
            return View();
        }
    }
}
