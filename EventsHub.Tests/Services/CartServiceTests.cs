using Application.Carts;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Moq;

namespace EventsHub.Tests.Services
{
    public class CartServiceTests
    {
        [Fact]
        public async Task CreateCart_WithExistingCart_ReturnsCartId()
        {
            // Arrange
            var userId = 1;
            var existingCart = new Cart { Id = 1, UserId = userId, Status = enCartStatus.Active };
            var cartRepo = new Mock<ICartRepo>();

            cartRepo.Setup(repo => repo.GetCurrentCartByUserIdAsync(userId))
                .ReturnsAsync(existingCart);

            var cartService = new CartService(cartRepo.Object);

            // Act
            var cartId = await cartService.CreateCartAsync(userId);

            // Assert
            Assert.Equal(existingCart.Id, cartId);
            
            cartRepo.Verify(repo => repo.GetCurrentCartByUserIdAsync(userId), Times.Once);
            cartRepo.Verify(repo => repo.AddAsync(It.IsAny<Cart>()), Times.Never);

        }

        [Fact]
        public async Task CreateCart_WithoutExistingCart_CreatesNewCart()
        {
            // Arrange
            var userId = 1;
            var newCart = new Cart { Id = 1, UserId = userId, Status = enCartStatus.Active };
            var cartRepo = new Mock<ICartRepo>();

            cartRepo.Setup(repo => repo.GetCurrentCartByUserIdAsync(userId))
                .ReturnsAsync((Cart?) null);

            cartRepo.Setup(repo => repo.AddAsync(It.IsAny<Cart>()))
                .ReturnsAsync(newCart);

            var cartService = new CartService(cartRepo.Object);

            // Act
            var cartId = await cartService.CreateCartAsync(userId);

            // Assert
            Assert.Equal(newCart.Id, cartId);

            cartRepo.Verify(repo => repo.GetCurrentCartByUserIdAsync(userId), Times.Once);
            cartRepo.Verify(repo => repo.AddAsync
                (It.Is<Cart>(cart =>
                    cart.UserId == userId &&
                    cart.Status == enCartStatus.Active &&
                    cart.TotalAmount == 0)), Times.Once);

        }

        [Fact]
        public async Task CreateCart_WithInvalidUserId_ThrowsArgumentOutOfRangeException()
        {             
            // Arrange
            var invalidUserId = -1;
            var cartRepo = new Mock<ICartRepo>();
            var cartService = new CartService(cartRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => cartService.CreateCartAsync(invalidUserId));
            cartRepo.Verify(repo => repo.GetCurrentCartByUserIdAsync(It.IsAny<int>()), Times.Never);
            cartRepo.Verify(repo => repo.AddAsync(It.IsAny<Cart>()), Times.Never);
        }

        [Fact]
        public async Task MarkCardAsOrdered_WithExistingCart_UpdatesStatus()
        {
            // Arrange 
            var userId = 1;
            var existingCart = new Cart
            {
                Id = 1,
                UserId = userId,
                Status = enCartStatus.Active
            };
            var cartRepo = new Mock<ICartRepo>();

            cartRepo.Setup(repo => repo.GetCurrentCartByUserIdAsync(userId))
                .ReturnsAsync(existingCart);

            cartRepo.Setup(repo => repo.UpdateAsync(existingCart))
                .ReturnsAsync(true);

            var cartService = new CartService(cartRepo.Object);

            // Act
            await cartService.MarkCartAsOrdered(userId);

            // Assert
            Assert.Equal(enCartStatus.Ordered, existingCart.Status);

            cartRepo.Verify(repo => repo.GetCurrentCartByUserIdAsync(userId), Times.Once);
            cartRepo.Verify(repo => repo.UpdateAsync(existingCart), Times.Once());

        }

        [Fact]
        public async Task MarkCartAsOrdered_WithoutExistingCart_ThrowsKeyNotFoundException()
        {
            var userId = 1;

            var cartRepo = new Mock<ICartRepo>();

            cartRepo.Setup(repo => repo.GetCurrentCartByUserIdAsync(userId))
                .ReturnsAsync((Cart?) null);

            var cartService = new CartService(cartRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => cartService.MarkCartAsOrdered(userId));

            cartRepo.Verify(repo => repo.GetCurrentCartByUserIdAsync(userId), Times.Once);
            cartRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Cart>()), Times.Never);

        }
        [Fact]
        public async Task MarkCardAsOrdered_WithInvalidUserId_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var invalidUserId = -1;
            var cartRepo = new Mock<ICartRepo>();
            var cartService = new CartService(cartRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => cartService.MarkCartAsOrdered(invalidUserId));
            cartRepo.Verify(repo => repo.GetCurrentCartByUserIdAsync(It.IsAny<int>()), Times.Never);
            cartRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Cart>()), Times.Never);

        }

    }
}
