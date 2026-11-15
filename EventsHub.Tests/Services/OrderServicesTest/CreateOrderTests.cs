using Application.Carts;
using Application.Orders;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsHub.Tests.Services.OrderServicesTest
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepo> _orderRepo;
        private readonly Mock<IOrderItemRepo> _orderItemRepo;
        private readonly Mock<ITicketTypeRepo> _ticketTypeRepo;
        private readonly Mock<ICartService> _cartService;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        private OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepo = new Mock<IOrderRepo>();
            _orderItemRepo = new Mock<IOrderItemRepo>();
            _ticketTypeRepo = new Mock<ITicketTypeRepo>();
            _cartService = new Mock<ICartService>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _orderService = new OrderService(
                _orderRepo.Object,
                _orderItemRepo.Object,
                _ticketTypeRepo.Object,
                _cartService.Object,
                _unitOfWork.Object
            );
        }

    }
}
