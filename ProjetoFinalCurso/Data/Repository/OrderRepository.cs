using Microsoft.EntityFrameworkCore;
using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Helpers;
using ProjetoFinalCurso.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjetoFinalCurso.Data.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public IQueryable GetOrdersSimple()
        {
            return _context.Orders
                .Include(c => c.Items)
                .ThenInclude(c=> c.Ticket)
                .Include(c => c.User)
                .Include(c => c.User);
        }

        public async Task<IQueryable<Order>> GetOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserbyEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Orders
                    .Include(u => u.User)
                    .Include(o => o.Items)
                    .ThenInclude(p => p.Ticket)
                    .OrderByDescending(o => o.OrderDate);
            }

            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(p => p.Ticket)
                .Include(o => o.User)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }


        public async Task<IQueryable<OrderDetailsTemp>> GetDetailsTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserbyEmailAsync(userName);
            if (user == null)
            {
                return null;
            }


            return _context.OrderDetailsTemps
                .Include(p => p.Ticket)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.Ticket.Concerto.Id);
        }

        public async Task<bool> AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserbyEmailAsync(userName);

            if (user == null)
            { return false; }

            var product = await _context.Tickets.FindAsync(model.TicketId);

            if (product == null)
            {
                return false;
            }

            if (product.Stock - model.Quantity < 0)
            {
                return false;
            }

            var orderDetailTemp = await _context.OrderDetailsTemps
                .Where(odt => odt.User == user && odt.Ticket == product)
                .FirstOrDefaultAsync();

            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailsTemp
                {
                    Price = product.Price,
                    Ticket = product,
                    Quantity = model.Quantity,

                    User = user
                };

                _context.OrderDetailsTemps.Add(orderDetailTemp);
            }
            else
            {
                if (product.Stock - model.Quantity - orderDetailTemp.Quantity < 0)
                {
                    return false;
                }

                orderDetailTemp.Quantity += model.Quantity;
                _context.OrderDetailsTemps.Update(orderDetailTemp);
            }

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task ModifyOrderDetailTempQuantity(int id, int quantity)
        {
            var orderDetailTemp = await _context.OrderDetailsTemps
                .Include(p => p.Ticket)
                .Where(o => o.Id == id)
                .OrderByDescending(o => o.Ticket.Concerto.Id)
                .FirstOrDefaultAsync();
            if (orderDetailTemp == null)
            {
                return;
            }



            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0 && orderDetailTemp.Ticket.Stock - orderDetailTemp.Quantity >= 0)
            {
                _context.OrderDetailsTemps.Update(orderDetailTemp);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteDetailtempAsync(int id)
        {
            var orderDetailTemp = await _context.OrderDetailsTemps.FindAsync(id);

            if (orderDetailTemp == null)
            {
                return;
            }

            _context.OrderDetailsTemps.Remove(orderDetailTemp);
            await _context.SaveChangesAsync();

        }


        public async Task<Order> ConfirmOrderAsync(string username, PaymentMethod paymentMethod)
        {
            var user = await _userHelper.GetUserbyEmailAsync(username);
            if (user == null)
            {
                Order o = new Order();
                return o;
            }

            var orderTmps = await _context.OrderDetailsTemps
                .Include(o => o.Ticket)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                Order o = new Order();
                return o;
            }

            foreach (var o in orderTmps)
            {
                if (o.Ticket.Stock - o.Quantity < 0)
                {
                    Order i = new Order();
                    return i;
                }
            }

            foreach (var o in orderTmps)
            {
                o.Ticket.Stock -= o.Quantity;

                _context.Tickets.Update(o.Ticket);
                
            }

            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Ticket = o.Ticket,
                Quantity = o.Quantity,

            }).ToList();


            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
                Status = "Awaiting Payment",
                PaymentMethod = paymentMethod,
            };

            await CreateAsync(order);

            _context.OrderDetailsTemps.RemoveRange(orderTmps);
            await _context.SaveChangesAsync();

            return order;


        }

        public async Task<Order> DeliveryOrder(Order model)
        {
            var order = await _context.Orders.FindAsync(model.Id);

            var order2 = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(o => o.Ticket)
                .Include(o => o.User)
                .Where(o => o.Id == model.Id).FirstOrDefaultAsync();

            if (order == null)
            {
                return (model);
            }

            order.Status = "Payment Concluded";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            model.Items = order2.Items;
            model.User = order2.User;
            model.Status = order.Status;

            return (model);
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(p => p.Ticket)
                .Include(o => o.User)
                .Where(o => o.Id == id)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteOrder(Order model)
        {
            var order = await _context.Orders.FindAsync(model.Id);

            if (order == null)
            {
                return;
            }

            order.Status = "Canceled";
            _context.Orders.Update(order);

            var order2 = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(o => o.Ticket)
                .Where(o => o.Id == model.Id).FirstOrDefaultAsync();

            var list2 = new List<Ticket>();

            foreach (var item in order2.Items)
            {
                list2.Add(item.Ticket);
            }

            foreach (var item in list2)
            {
                foreach (var item2 in order2.Items)
                {
                    if(item.Id == item2.Ticket.Id)
                    {
                        item.Stock += item2.Quantity;
                        _context.Set<Ticket>().Update(item);
                    }

                    
                }
                
            }

            await _context.SaveChangesAsync();
        }

        public IEnumerable<SelectListItem> GetComboPayments()
        {
            var list = _context.PaymentMethods.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Please select a payment method...)",
                Value = "0",
            });

            return list;
        }

        public async Task<OrderViewModel> AddPaymentToOrderAsync(OrderViewModel model)
        {
            
            var payment = await _context.PaymentMethods.FindAsync(model.PaymentId);

            if (payment == null)
            {
                return model;
            }

            model.PaymentMethod = payment;


            return model;
        }

        public async Task<int> VerifyOrderAsync(string username)
        {
            var user = await _userHelper.GetUserbyEmailAsync(username);
            if (user == null)
            {
                return 1;
            }

            var orderTmps = await _context.OrderDetailsTemps
                .Include(o => o.Ticket)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return 1;
            }

            foreach (var o in orderTmps)
            {
                if (o.Ticket.Stock - o.Quantity < 0)
                {
                    return 2;
                }
            }

            return 3;
        }
    }
}
