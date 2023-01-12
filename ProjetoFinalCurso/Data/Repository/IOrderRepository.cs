using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalCurso.Data.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        IQueryable GetOrdersSimple();

        Task<IQueryable<Order>> GetOrderAsync(string userName);

        Task<IQueryable<OrderDetailsTemp>> GetDetailsTempsAsync(string userName);

        Task<bool> AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task<OrderViewModel> AddPaymentToOrderAsync(OrderViewModel model);

        Task ModifyOrderDetailTempQuantity(int id, int quantity);

        Task DeleteDetailtempAsync(int id);

        Task<Order> ConfirmOrderAsync(string username, PaymentMethod paymentMethod);

        Task<int> VerifyOrderAsync(string username);

        Task<Order> DeliveryOrder(Order model);

        Task DeleteOrder(Order model);

        Task<Order> GetOrderAsync(int id);

        IEnumerable<SelectListItem> GetComboPayments();
    }
}
