using Application.CartItems;
using Application.CartItems.Dto.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Moq;

namespace EventsHub.Tests.Services.CartItemServiceTests
{
    public partial class CartItemServiceTests
    {

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task OwnershipCheckAsync_WithInvalidUserId_ThrowsArgumentOutOfRangeException(int userId)
        {
            var (_, _, _, _, itemService) = await Arrange();

            var itemId = 1;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => itemService.OwnershipCheckAsync(userId, itemId));
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task OwnershipCheckAsync_WithInvalidItemId_ThrowsArgumentOutOfRangeException(int itemId)
        {
            var (_, _, _, _, itemService) = await Arrange();

            var userId = 1; 

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => itemService.OwnershipCheckAsync(userId, itemId));
        }
        [Fact]
        public async Task OwnershipCheckAsync_WhenUserIsNotOwner_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();
            var cart = CreateCart();
            var userId = 2;
            var itemId = 1;
            var existingItem = cart.CartItems.First(i => i.Id == itemId);
            existingItem.Cart = cart;

            itemRepo.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync(existingItem);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                itemService.OwnershipCheckAsync(userId, itemId));

            itemRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);

        }

        [Fact]
        public async Task OwnershipCheckAsync_WhenItemNotExisted_ThrowsKeyNotFoundException()
        {
            // Arrange
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();
            var userId = 2;
            var itemId = 1;

            itemRepo.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync((CartItem?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                itemService.OwnershipCheckAsync(userId, itemId));

            itemRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);

        }

        [Fact]
        public async Task OwnershipCheckAsync_WhenCartNotExisted_ThrowsKeyNotFoundException()
        {
            // Arrange
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();
            var cart = CreateCart();
            var userId = 2;
            var itemId = 1;
            var existingItem = cart.CartItems.First(i => i.Id == itemId);
            existingItem.Cart = null;

            itemRepo.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync(existingItem);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                itemService.OwnershipCheckAsync(userId, itemId));

            itemRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateItemQtyAsync_WithInValidUserId_ThrowsArgumentOutOfRangeException(int userId)
        {
            // Arrange
            var (_, _, _, _, itemService) = await Arrange();
            var itemId = 1;
            var newQuantity = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                itemService.UpdateItemQuantityAsync(userId, itemId, newQuantity));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateItemQtyAsync_WithInValidItemId_ThrowsArgumentOutOfRangeException(int itemId)
        {
            // Arrange
            var (_, _, _, _, itemService) = await Arrange();
            var userId = 1;
            var newQuantity = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                itemService.UpdateItemQuantityAsync(userId, itemId, newQuantity));
        }

        [Fact]
        public async Task UpdateItemQtyAsync_WithInvalidQuantity_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var (_, _, _, _, itemService) = await Arrange();
            var userId = 1;
            var itemId = 1;
            var newQuantity = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                itemService.UpdateItemQuantityAsync(userId, itemId, newQuantity));
        }
        [Fact]
        public async Task UpdateItemQtyAsync_WhenSucceed_UpdatesCartItemQuantity()
        {
            // Arrange
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();
            var cart = CreateCart();
            var userId = 1;
            var itemId = 1;
            var newQuantity = 3;
            var existingItem = cart.CartItems.First(i => i.Id == itemId);
            existingItem.Cart = cart;
            var increasedQuantity = newQuantity - existingItem.Quantity;

            itemRepo.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync(existingItem);

            ticketTypeRepo.Setup(r => r.IsThereAvaliableTicketsAsync(existingItem.TicketTypeId, increasedQuantity))
                .ReturnsAsync(true);

            itemRepo.Setup(r => r.CalculateCartTotalPrice(cart.Id))
                .ReturnsAsync(180);

            cartRepo.Setup(r => r.UpdateTotalAmount(cart.Id, 180))
                .ReturnsAsync(true);

            // Act
            await itemService.UpdateItemQuantityAsync(userId, itemId, newQuantity);

            // Assert

            Assert.Equal(newQuantity, existingItem.Quantity);

            ticketTypeRepo.Verify(
                r => r.IsThereAvaliableTicketsAsync(existingItem.TicketTypeId, increasedQuantity), Times.Once);

            itemRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);

            itemRepo.Verify(
                r => r.UpdateAsync(
                    It.Is<CartItem>
                    (
                        i => i.Id == itemId &&
                        i.Quantity == existingItem.Quantity
                    )),
                Times.Once);

            cartRepo.Verify(r => r.UpdateTotalAmount(cart.Id, It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public async Task UpdateItemQtyAsync_WhenQtyIsTheSame_Return()
        {
            // Arrange
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();
            var cart = CreateCart();
            var userId = 1;
            var itemId = 1;
            var newQuantity = 2;
            var existingItem = cart.CartItems.First(i => i.Id == itemId);
            existingItem.Cart = cart;

            itemRepo.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync(existingItem);

            // Act
            await itemService.UpdateItemQuantityAsync(userId, itemId, newQuantity);

            // Assert
            Assert.Equal(newQuantity, existingItem.Quantity);

            ticketTypeRepo.Verify(
                r => r.IsThereAvaliableTicketsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            itemRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);

            itemRepo.Verify(
                r => r.UpdateAsync(It.IsAny<CartItem>()), Times.Never);
            cartRepo.Verify(r => r.UpdateTotalAmount(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public async Task UpdateItemQtyAsync_WhenThereIsNotEnoughTickets_ThrowsNotEnoughTicketException()
        {
            // Arrange
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();
            var cart = CreateCart();
            var userId = 1;
            var itemId = 1;
            var newQuantity = 5;
            var existingItem = cart.CartItems.First(i => i.Id == itemId);
            existingItem.Cart = cart;
            var increasedQuantity = newQuantity - existingItem.Quantity;
            
            itemRepo.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync(existingItem);
           
            ticketTypeRepo.Setup(r => r.IsThereAvaliableTicketsAsync(existingItem.TicketTypeId, increasedQuantity))
                .ReturnsAsync(false);
            // Act & Assert
            await Assert.ThrowsAsync<NotEnoughTicketException>(() =>
                itemService.UpdateItemQuantityAsync(userId, itemId, newQuantity));
            ticketTypeRepo.Verify(
                r => r.IsThereAvaliableTicketsAsync(existingItem.TicketTypeId, increasedQuantity), Times.Once);
            itemRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);
            itemRepo.Verify(
                r => r.UpdateAsync(It.IsAny<CartItem>()), Times.Never);
            cartRepo.Verify(r => r.UpdateTotalAmount(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public async Task UpdateItemQtyAsync_WhenQtyIsZero_DeletesCartItem()
        {
            // Arrange
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();
            var cart = CreateCart();
            var userId = 1;
            var itemId = 1;
            var newQuantity = 0;
            var existingItem = cart.CartItems.First(i => i.Id == itemId);
            existingItem.Cart = cart;

            itemRepo.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync(existingItem);

            itemRepo.Setup(r => r.CalculateCartTotalPrice(cart.Id))
                .ReturnsAsync(30);

            cartRepo.Setup(r => r.UpdateTotalAmount(cart.Id, 30))
                .ReturnsAsync(true);
            // Act
            await itemService.UpdateItemQuantityAsync(userId, itemId, newQuantity);
            // Assert
            itemRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);
            itemRepo.Verify(r => r.DeleteAsync(existingItem), Times.Once);
            itemRepo.Verify(r => r.UpdateAsync(existingItem), Times.Never);
            cartRepo.Verify(r => r.UpdateTotalAmount(cart.Id, 30), Times.Once);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteItemAsync_WithInValidUserId_ThrowsArgumentOutOfRangeException(int userId)
        {
            // Arrange
            var (_, _, _, _, itemService) = await Arrange();
            var itemId = 1;
            var newQuantity = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                itemService.DeleteAsync(userId, itemId));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteItemAsync_WithInValidItemId_ThrowsArgumentOutOfRangeException(int itemId)
        {
            // Arrange
            var (_, _, _, _, itemService) = await Arrange();
            var userId = 1;
            var newQuantity = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                itemService.DeleteAsync(userId, itemId));
        }

        [Fact]
        public async Task DeleteItemAsync_WhenSucceed_DeletesCartItem()
        {
            // Arrange
            var (itemRepo, cartRepo, ticketTypeRepo, cartService, itemService) = await Arrange();
            var cart = CreateCart();
            var userId = 1;
            var itemId = 1;
            var existingItem = cart.CartItems.First(i => i.Id == itemId);
            existingItem.Cart = cart;

            itemRepo.Setup(r => r.GetByIdAsync(itemId))
                .ReturnsAsync(existingItem);

            itemRepo.Setup(r => r.CalculateCartTotalPrice(cart.Id))
                .ReturnsAsync(30);

            cartRepo.Setup(r => r.UpdateTotalAmount(cart.Id, 30))
                .ReturnsAsync(true);
            // Act
            await itemService.DeleteAsync(userId, itemId);

            // Assert
            itemRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);
            itemRepo.Verify(r => r.DeleteAsync(existingItem), Times.Once);
            cartRepo.Verify(r => r.UpdateTotalAmount(cart.Id, 30), Times.Once);
        }

    }

}
