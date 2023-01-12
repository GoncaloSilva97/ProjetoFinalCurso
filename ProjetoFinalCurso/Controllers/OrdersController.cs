using Microsoft.AspNetCore.Mvc;
using ProjetoFinalCurso.Data.Repository;
using ProjetoFinalCurso.Models;
using System.Threading.Tasks;
using System;
using ProjetoFinalCurso.Helpers;
using ProjetoFinalCurso.Data.Entities;
using System.Linq;

namespace ProjetoFinalCurso.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IMailHelper _mailHelper;

        public OrdersController(IOrderRepository orderRepository, ITicketRepository ticketRepository, IUserHelper userHelper,
            IConverterHelper converterHelper, IMailHelper mailHelper)
        {
            _orderRepository = orderRepository;
            _ticketRepository = ticketRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _mailHelper = mailHelper;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _orderRepository.GetOrderAsync(this.User.Identity.Name);

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _orderRepository.GetDetailsTempsAsync(this.User.Identity.Name);

            

            return View(model);
        }

        public IActionResult AddTicket()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                
                Concerts = _ticketRepository.GetComboAllConcert(),
                Tickets = _ticketRepository.GetComboTickets(),
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddTicket(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool b = await _orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);

                if (b == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    ViewBag.Message = "Quantity exceeds ticket stock.";
                   
                }
                
            }

            var model1 = new AddItemViewModel
            {
                Quantity = 1,
                Tickets = _ticketRepository.GetComboTickets(),
                Concerts = _ticketRepository.GetComboAllConcert()
            };

            return View(model1);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OrderNotFound");
            }

            await _orderRepository.DeleteDetailtempAsync(id.Value);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OrderNotFound");
            }

            await _orderRepository.ModifyOrderDetailTempQuantity(id.Value, 1);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OrderNotFound");
            }

            await _orderRepository.ModifyOrderDetailTempQuantity(id.Value, -1);
            return RedirectToAction("Create");
        }


        public async Task<IActionResult> ConfirmOrder()
        {
            var order = await _orderRepository.VerifyOrderAsync(this.User.Identity.Name);
            if (order == 3)
            {

                return View(new OrderViewModel
                {
                    
                    PaymentId = 1,
                    Payments = _orderRepository.GetComboPayments()
                });
            }
            else 
            {
                return RedirectToAction("Create");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(OrderViewModel model)
        {
            var o = await _orderRepository.VerifyOrderAsync(this.User.Identity.Name);
            if (o == 2)
            {

                ViewBag.Message = "Quantity exceeds ticket stock, please go back and change your order.";
                return View(model);
            }
            else if (o == 1)
            {
                ViewBag.Message = "Your tickets have alrerady been sent to your email";
                return View(model);
            }

           

            if (ModelState.IsValid)
            {
                model = await _orderRepository.AddPaymentToOrderAsync(model);

                
                var order = await _orderRepository.ConfirmOrderAsync(this.User.Identity.Name, model.PaymentMethod);


                string str = "";
                string str2 = "";

                foreach (var item in order.Items)
                {
                    str += item.Ticket.Id + " " + item.Ticket.Code + " ";
                }

                if(order.PaymentMethod.Name == "Bank Transfer")
                {
                    str2 = $"Please transfer {order.Value} to the following account: {order.PaymentMethod.Info}";
                }
                else if (order.PaymentMethod.Name == "MBWay")
                {
                    str2 = $"Please transfer {order.Value} to the following number: {order.PaymentMethod.Info}";
                }

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(order.User);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = order.User.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(order.User.UserName, "NFG Tickets", $"<h1>NFG Tickets Order {order.Id}</h1>" +
                    $"Ticket Code(s): /n{str}." + $"/n{str2}.");


                if (response.IsSuccess)
                {
                    ViewBag.Message = "Your tickets have been sent to your email";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "E-mail couldnt be Sent.");

                return View(model);
            }



            return View(model);

        }
        public async Task<IActionResult> Deliver(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OrderNotFound");
            }

            var order = await _orderRepository.GetOrderAsync(id.Value);



            return View(order);

        }

        [HttpPost]
        public async Task<IActionResult> Deliver(Order model)
        {
            if (ModelState.IsValid)
            {
                if (model.Status == "Canceled" || model.Status == null)
                {
                    return new NotFoundViewResult("OrderNotFound");
                }

                model = await _orderRepository.GetOrderAsync(model.Id);

                model = await _orderRepository.DeliveryOrder(model);
                string str = "";

                foreach (var item in model.Items)
                {
                    str += item.Ticket.Id + " " + item.Ticket.Code + " ";
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(model.User);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = model.User.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.User.UserName, "NFG Tickets", $"<h1>NFG Tickets Order</h1>" +
                    $"Ticket Code(s): {str}");


                if (response.IsSuccess)
                {
                    ViewBag.Message = "The tickets have been sent to the users email";
                    return View();
                }

                ModelState.AddModelError(string.Empty, "E-mail couldnt be Sent.");

                return View();
            }



            return View(model);
        }

        public async Task<IActionResult> DeleteOrder(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OrderNotFound");
            }

            var order = await _orderRepository.GetOrderAsync(id.Value);

            if (order.Status == "Canceled")
            {
                return new NotFoundViewResult("OrderNotFound");
            }



            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {

            var product = await _orderRepository.GetByIdAsync(id);

            await _orderRepository.DeleteOrder(product);

            return RedirectToAction("Index");

        }

        [HttpPost]
        [Route("Orders/GetTicketsAsync")]
        public async Task<JsonResult> GetTicketsAsync(int concertId)
        {
            var seats = await _ticketRepository.VerifyTickets(concertId);


            return Json(seats.OrderBy(c => c.Text));
        }

        public IActionResult OrderNotFound()
        {
            return View();
        }
    }
}
