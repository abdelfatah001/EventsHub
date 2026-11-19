using Application.Carts;
using Application.Orders;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IMapper _mapper;

        private OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepo = new Mock<IOrderRepo>();
            _orderItemRepo = new Mock<IOrderItemRepo>();
            _ticketTypeRepo = new Mock<ITicketTypeRepo>();
            _cartService = new Mock<ICartService>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = null;

            _orderService = new OrderService(
                _orderRepo.Object,
                _orderItemRepo.Object,
                _ticketTypeRepo.Object,
                _cartService.Object,
                _unitOfWork.Object,
                _mapper
            );
        }

        private Cart CreateCartInstance ()
        {

            var Items = new Collection<CartItem>()
            {
                new CartItem { Id = 1 , CartId = 1, Quantity = 2, UnitPrice = 10, TicketTypeId = 1},
                new CartItem { Id = 2, CartId = 1, Quantity = 3, UnitPrice = 30, TicketTypeId = 2 }
            };

            var cart = new Cart
            {
                Id = 1,
                UserId = 1,
                TotalAmount = 110,
                Status = Domain.Enums.enCartStatus.Active,
                CartItems = Items
            };

            return cart;
        }

        [Fact]
        public async Task CreateOrder_WhenCreated_CreateOrderAndMarkCartAsOrdered ()
        {
            // Arrange

            int userId = 1;

            var cart = CreateCartInstance();

             Order? createdOrder = null;

            _cartService
                .Setup(s => s.GetCurrentCartByUserIdAsync(userId))
                .ReturnsAsync(cart);

            foreach (var item in cart.CartItems)
            {
                _ticketTypeRepo
                    .Setup(r => r.TryReserveAsync(item.TicketTypeId, item.Quantity))
                    .ReturnsAsync(item.UnitPrice);
            }

            _unitOfWork
                .Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWork
                .Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);


            // Act
            var result = await _orderService.CreateOrderAsync(userId);

            // Assert
            Assert.NotNull(result); 
            Assert.Equal(userId, result.UserId);
            Assert.Equal(Domain.Enums.enOrderStatus.PendingPayment, result.Status);
            Assert.Equal(110, result.TotalAmount);
            Assert.Equal(2, result.OrderItems.Count);


            _cartService.Verify(s => s.GetCurrentCartByUserIdAsync(userId), Times.Once);

            _orderRepo.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);

            _ticketTypeRepo.Verify(r => r.TryReserveAsync(It.IsAny<int>(), It.IsAny<int>()),
                Times.Exactly(2));

            _orderItemRepo.Verify(r => r.Add(It.IsAny<OrderItem>()), Times.Exactly(2));

            _cartService.Verify(s => s.MarkCartAsOrdered(userId), Times.Once);

            // Transaction
            _unitOfWork.Verify(
                u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(
                u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(
                u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateOrder_WhenPriceChanged_RecalculateTheOrderTotalAmount()
        {
            int userId = 1;

            var cart = CreateCartInstance();

            Order? createdOrder = null;

            _cartService
                .Setup(s => s.GetCurrentCartByUserIdAsync(userId))
                .ReturnsAsync(cart);

            foreach (var item in cart.CartItems)
            {
                if (item.Id == 2)
                {
                    _ticketTypeRepo
                        .Setup(r => r.TryReserveAsync(item.TicketTypeId, item.Quantity))
                        .ReturnsAsync(50);
                }
                else
                {
                    _ticketTypeRepo
                        .Setup(r => r.TryReserveAsync(item.TicketTypeId, item.Quantity))
                        .ReturnsAsync(item.UnitPrice);
                }
            }

            _unitOfWork
                .Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWork
                .Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);


            // Act
            var result = await _orderService.CreateOrderAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(Domain.Enums.enOrderStatus.PendingPayment, result.Status);
            Assert.Equal(170, result.TotalAmount);
            Assert.Equal(2, result.OrderItems.Count);


            _cartService.Verify(s => s.GetCurrentCartByUserIdAsync(userId), Times.Once);

            _orderRepo.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);

            _ticketTypeRepo.Verify(r => r.TryReserveAsync(It.IsAny<int>(), It.IsAny<int>()),
                Times.Exactly(2));

            _orderItemRepo.Verify(r => r.Add(It.IsAny<OrderItem>()), Times.Exactly(2));

            _cartService.Verify(s => s.MarkCartAsOrdered(userId), Times.Once);

            // Transaction
            _unitOfWork.Verify(
                u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(
                u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(
                u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateOrder_WhenThereIsNoEnoughTickets_ThrowsNotEnoughTicketException()
        {
            int userId = 1;

            var cart = CreateCartInstance();

            Order? createdOrder = null;

            _cartService
                .Setup(s => s.GetCurrentCartByUserIdAsync(userId))
                .ReturnsAsync(cart);

            foreach (var item in cart.CartItems)
            {
                if (item.Id == 2)
                {
                    _ticketTypeRepo
                        .Setup(r => r.TryReserveAsync(item.TicketTypeId, item.Quantity))
                        .ReturnsAsync((decimal?)null);
                }
                else
                {
                    _ticketTypeRepo
                        .Setup(r => r.TryReserveAsync(item.TicketTypeId, item.Quantity))
                        .ReturnsAsync(item.UnitPrice);
                }
            }

            _unitOfWork
                .Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);


            _unitOfWork
                .Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotEnoughTicketException>(
                    () => _orderService.CreateOrderAsync(userId));

            Assert.NotNull(exception);

            _cartService.Verify(s => s.GetCurrentCartByUserIdAsync(userId), Times.Once);
            

            _orderRepo.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);

            _ticketTypeRepo.Verify(r => r.TryReserveAsync(It.IsAny<int>(), It.IsAny<int>()),
                Times.Exactly(2));

            _orderItemRepo.Verify(r => r.Add(It.IsAny<OrderItem>()), Times.Once);

            _cartService.Verify(s => s.MarkCartAsOrdered(userId), Times.Never);

            // Transaction
            _unitOfWork.Verify(
                u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(
                u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWork.Verify(
                u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Never);

        }

        [Fact]
        public async Task CreateOrder_WhenThereIsNoActiveCart_ThrowsBusinessException()
        {
            // Arrange
            int userId = 1;

            _cartService
                .Setup(s => s.GetCurrentCartByUserIdAsync(userId))
                .ReturnsAsync((Cart?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(
                () => _orderService.CreateOrderAsync(userId));

            _cartService.Verify(
                s => s.GetCurrentCartByUserIdAsync(userId),
                Times.Once);

            _orderRepo.Verify(
                r => r.Add(It.IsAny<Order>()),
                Times.Never);

            _unitOfWork.Verify(
                u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateOrder_WhenCartIsEmpty_ThrowsBusinessException()
        {
            // Arrange
            int userId = 1;

            var cart = new Cart
            {
                Id = 1,
                UserId = userId,
                TotalAmount = 0,
                Status = enCartStatus.Active,
                CartItems = new Collection<CartItem>()
            };

            _cartService
                .Setup(s => s.GetCurrentCartByUserIdAsync(userId))
                .ReturnsAsync(cart);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(
                () => _orderService.CreateOrderAsync(userId));

            _cartService.Verify(
                s => s.GetCurrentCartByUserIdAsync(userId),
                Times.Once);

            _orderRepo.Verify(
                r => r.Add(It.IsAny<Order>()),
                Times.Never);

            _unitOfWork.Verify(
                u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }

}
