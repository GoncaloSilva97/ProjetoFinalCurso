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
    public class TicketsController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IFlashMessage _flashMessage;

        public TicketsController(ITicketRepository ticketRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper
            , IFlashMessage flashMessage)
        {
            _ticketRepository = ticketRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _flashMessage = flashMessage;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            

            if (this.User.Identity.Name == null)
            {
                return View(_ticketRepository.GetTicketsSimple());
            }

            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

            bool b = false;

            if (this.User.IsInRole("Admin") || this.User.IsInRole("Customer"))
            {
                b = true;
            }

            return View(_ticketRepository.GetTicketsWithConcert(b, user));
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            var model = await _ticketRepository.GetbyIdTicketsWithConcertAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            return View(model);
        }
        [Authorize(Roles = "Admin, Employee")]
        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

            

            var model = new TicketViewModel
            {
                ListConcerts = _ticketRepository.GetComboConcert(this.User.IsInRole("Admin"), user),
                ListTicketTypes = _ticketRepository.GetComboTicketTypes(),
            };

            //model.model = new Model();

            return View(model);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModel ticket)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                ticket = await _ticketRepository.AddConcertAsync(ticket);

                var product = _converterHelper.toTicket(ticket, imageId, true);

                product.Code = product.Concerto.Title + " " + product.Type.Name;

                product.User = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);

                try
                {
                    await _ticketRepository.CreateAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    //_flashMessage.Danger("This country already exist!");
                }

            }
            return View(ticket);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            var model = await _ticketRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            var user = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);



            var viewmodel = _converterHelper.toTicketViewModel(model);
            viewmodel.ListConcerts = _ticketRepository.GetComboConcert(this.User.IsInRole("Admin"), user);
            viewmodel.ListTicketTypes = _ticketRepository.GetComboTicketTypes();

            return View(viewmodel);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TicketViewModel ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    
                    var product = _converterHelper.toTicket(ticket, imageId, false);


                    product.User = await _userHelper.GetUserbyEmailAsync(this.User.Identity.Name);
                    await _ticketRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _ticketRepository.ExistAsync(ticket.Id))
                    {
                        return new NotFoundViewResult("TicketNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            var model = await _ticketRepository.GetbyIdTicketsWithConcertAsync(id.Value);
            if (model == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            return View(model);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _ticketRepository.GetByIdAsync(id);

            try
            {
                await _ticketRepository.DeleteAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{product.Code} is probably being used!!";
                    ViewBag.ErrorMessage = $"{product.Code} can´t be delete since its being used in a order.</br></br>" +
                       $"First delete the orders that are using it then try again.";
                }


                return View("Error");

            }
        }

        public IActionResult TicketNotFound()
        {
            return View();
        }

        [HttpPost]
        [Route("Tickets/GetTypesAsync")]
        public async Task<JsonResult> GetTypesAsync(int concertId)
        {
            var seats = await _ticketRepository.VerifyType(concertId);


            return Json(seats.OrderBy(c => c.Value));
        }
    }
}
