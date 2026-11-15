using Application.CartItems;
using Application.CartItems.Dto.Request;
using Application.Carts;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Moq;

namespace EventsHub.Tests.Services.CartItemServiceTests
{
    public partial class CartItemServiceTests
    {
        public Cart CreateCart ()
        {
            var userId = 1;

            var CartItems = new List<CartItem>()
            {
                new CartItem
                {
                    Id = 1,
                    CartId = 1,
                    TicketTypeId = 1,
                    Quantity = 2,
                    UnitPrice = 50.00m
                },
                new CartItem
                {
                    Id = 2,
                    CartId = 1,
                    TicketTypeId = 2,
                    Quantity = 1,
                    UnitPrice = 30.00m
                }
            };

            var cart = new Cart()
            {
                Id = 1,
                UserId = userId,
                CartItems = CartItems,
                TotalAmount = CartItems.Sum(x => x.UnitPrice * x.Quantity)
            };


            return cart;

        }

        public async Task<(Mock<ICartItemRepo>, Mock<ICartRepo>, 
            Mock<ITicketTypeRepo>, Mock<ICartService>, CartItemService)>  Arrange ()
        {
            var itemRepo = new Mock<ICartItemRepo>();
            var cartRepo = new Mock<ICartRepo>();
            var tickeTypeRepo = new Mock<ITicketTypeRepo>();
            var mapper = new Mock<IMapper>();

            var cartService = new Mock<ICartService>();


            var itemService = new CartItemService(itemRepo.Object, cartRepo.Object,
                tickeTypeRepo.Object, cartService.Object, mapper.Object);

            return (itemRepo,  cartRepo, tickeTypeRepo, cartService, itemService);

        }


        [Fact]
        public async Task AddCartItem_WithExistingCart_AddsItemToCart()
        {
            // Arrange
            var userId = 1;
            var cart = CreateCart();
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();

            var itemDto = new AddCartItemDto
            {
                TicketTypeId = 3,
                Quantity = 5
            };

            var item = new CartItem
            {
                Id = 3,
                CartId = cart.Id,
                TicketTypeId = 3,
                Quantity = 5,
                UnitPrice = 10
            };
            
            ticketTypeRepo.Setup(repo => repo.IsThereAvaliableTicketsAsync(itemDto.TicketTypeId, item.Quantity))
                .ReturnsAsync(true);

            cartService.Setup(cartService => cartService.CreateCartAsync(userId))
                .ReturnsAsync(cart.Id);

            itemRepo.Setup(repo => repo.IsCartItemRepeated(cart.Id, itemDto.TicketTypeId))
                .ReturnsAsync((CartItem?)null);

            itemRepo.Setup(repo => repo.CalculateCartTotalPrice(item.CartId))
                .ReturnsAsync(180);

            // Act
            await itemService.AddAsync(userId, itemDto);


            // Assert

            ticketTypeRepo.Verify(repo => repo.IsThereAvaliableTicketsAsync(itemDto.TicketTypeId, itemDto.Quantity), Times.Once);

            cartService.Verify(cartService => cartService.CreateCartAsync(userId), Times.Once);

            itemRepo.Verify(itemRepo => itemRepo.IsCartItemRepeated(cart.Id, itemDto.TicketTypeId), Times.Once);

            itemRepo.Verify(repo => repo.AddAsync
                (It.Is<CartItem>(addedItem =>
                    addedItem.CartId == cart.Id &&
                    addedItem.TicketTypeId == itemDto.TicketTypeId &&
                    addedItem.Quantity == itemDto.Quantity)), 
                Times.Once);

            cartRepo.Verify(repo => repo.UpdateTotalAmount(
                cart.Id, 180), Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenItemAlreadyExists_IncreasesExistingItemQuantity()
        {
            // Arrange
            var userId = 1;
            var cart = CreateCart();

            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();

            var existingItem = cart.CartItems.First();
            existingItem.Cart = cart;

            var itemDto = new AddCartItemDto
            {
                TicketTypeId = 1,
                Quantity = 2
            };


            ticketTypeRepo.Setup(repo => repo.IsThereAvaliableTicketsAsync(itemDto.TicketTypeId, itemDto.Quantity))
                .ReturnsAsync(true);

            cartService.Setup(cartService => cartService.CreateCartAsync(userId))
                .ReturnsAsync(cart.Id);

            itemRepo.Setup(repo => repo.IsCartItemRepeated(cart.Id, itemDto.TicketTypeId))
                .ReturnsAsync(existingItem);

            itemRepo.Setup(repo => repo.CalculateCartTotalPrice(cart.Id))
                .ReturnsAsync(230);

            // Act
            await itemService.AddAsync(userId, itemDto);


            // Assert

            ticketTypeRepo.Verify(
                repo => repo.IsThereAvaliableTicketsAsync(itemDto.TicketTypeId, itemDto.Quantity), Times.Once);

            cartService.Verify(cartService => cartService.CreateCartAsync(userId), Times.Once);

            itemRepo.Verify(itemRepo => itemRepo.IsCartItemRepeated(cart.Id, itemDto.TicketTypeId), Times.Once);

            itemRepo.Verify(repo => repo.AddAsync(It.IsAny<CartItem>()), Times.Never);


            itemRepo.Verify(repo => repo.UpdateAsync(
                It.Is<CartItem>(addedItem =>
                        addedItem.CartId == cart.Id &&
                        addedItem.TicketTypeId == itemDto.TicketTypeId &&
                        addedItem.Quantity == 4)), 
                    Times.Once);

            cartRepo.Verify(repo => repo.UpdateTotalAmount(cart.Id, 230), Times.Once);
        }
        [Fact]
        public async Task AddAsync_WhenTicketsAreUnavailable_ThrowsNotEnoughTicketException()
        {
            // Arrange
            var userId = 1;

            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();

            var itemDto = new AddCartItemDto
            {
                TicketTypeId = 1,
                Quantity = 5
            };

            ticketTypeRepo
                .Setup(repo => repo.IsThereAvaliableTicketsAsync(
                    itemDto.TicketTypeId, itemDto.Quantity))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotEnoughTicketException>(
                () => itemService.AddAsync(userId, itemDto));

            cartService.Verify(
                service => service.CreateCartAsync(It.IsAny<int>()),
                Times.Never);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task AddAsync_WithInvalidUserId_ThrowsArgumentOutOfRangeException(int userId)
        {
            // Arrange

            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();

            var itemDto = new AddCartItemDto
            {
                TicketTypeId = 1,
                Quantity = 2
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => itemService.AddAsync(userId, itemDto));

        }

        [Fact]
        public async Task AddAsync_WithNullDto_ThrowsArgumentNullException()
        {
            // Arrange
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => itemService.AddAsync(1, null!));

        }

        
    }
}
