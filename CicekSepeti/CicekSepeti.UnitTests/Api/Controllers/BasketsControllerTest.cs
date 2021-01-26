using CicekSepeti.Api.Controllers;
using CicekSepeti.Business.Interfaces;
using CicekSepeti.Entities.ComplexTypes;
using CicekSepeti.Entities.ComplexTypes.Enum;
using CicekSepeti.Entities.DTOs;
using CicekSepeti.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CicekSepeti.UnitTests.Api.Controllers
{
    public class BasketsControllerTest
    {
        private readonly Mock<IBasketService> mockIBasketService;
        private readonly BasketsController basketsController;
        public BasketsControllerTest()
        {
            mockIBasketService = new Mock<IBasketService>(MockBehavior.Default);
            basketsController = new BasketsController(mockIBasketService.Object);
        }
       
        [Fact]
        public async Task AddOrSetBasketItem_StockCheck_ReturnOutOfStockError()
        {
            var basketWithItemDto = new BasketWithItemDto()
            {
                BasketIdx = "1cv1039a1521eaa34265e234",
                CustomerIdx = "1cv1039a1521eaa34265e234",
                ProductIdx = "1cv1039a1521eaa34265e234",
                Quantity = 1,
                SalePrice = (decimal)123.23
            };
            var expectedResult = new Result<BasketWithItemResponse>()
            {
                IsSuccessful = false,
                ErrorMessages = new List<ErrorModel>() { ErrorCodes.OutOfStock },
                EntityList = new List<BasketWithItemResponse>(),
                EntityCount = 0,
                IsValid = false
            };


            mockIBasketService.Setup(x => x.AddOrSetBasketItemAsync(basketWithItemDto)).ReturnsAsync(expectedResult);
            var actualResult = await basketsController.AddOrSetBasketItem(basketWithItemDto);


            var okResult = Assert.IsType<OkObjectResult>(actualResult);
            var resultmodal = (Result<BasketWithItemResponse>)okResult.Value;
            Assert.False(resultmodal.IsSuccessful);

            Assert.Equal(ErrorCodes.OutOfStock.Text, resultmodal.ErrorMessages
                .FirstOrDefault(x => x.Text == ErrorCodes.OutOfStock.Text)?.Text ?? null);

            Assert.Equal(expectedResult, resultmodal);

        }

        [Fact]
        public async Task AddOrSetBasketItem_InValidModelState_ReturnFormatIsWrong()
        {
            var basketWithItemDto = new BasketWithItemDto()
            {
                BasketIdx = "",
                CustomerIdx = "dfre544",
                ProductIdx = "1cv1039a1521eaa34265e234",
                Quantity = 1,
                SalePrice = (decimal)123.23
            };
            var expectedResult = new Result<BasketWithItemResponse>()
            {
                IsSuccessful = false,
                ErrorMessages = new List<ErrorModel>() {ErrorCodes.IdFormatIsWrong},
                EntityList = new List<BasketWithItemResponse>(),
                EntityCount = 0,
                IsValid = false
            };


            mockIBasketService.Setup(x => x.AddOrSetBasketItemAsync(basketWithItemDto)).ReturnsAsync(expectedResult);
            var actualResult = await basketsController.AddOrSetBasketItem(basketWithItemDto);


            var okResult = Assert.IsType<OkObjectResult>(actualResult);
            var resultmodal = Assert.IsType<Result<BasketWithItemResponse>>(okResult.Value);
            Assert.Equal(400, resultmodal.ErrorMessages.FirstOrDefault(x => x.Code == 400)?.Code ?? 0);
            Assert.Equal(expectedResult, resultmodal);

        }

        [Fact]
        public async Task AddOrSetBasketItem_NewBasketSuccessfullyRegistered_ReturnSuccessfulAndRecordedResult()
        {
            var basketWithItemDto = new BasketWithItemDto()
            {
                BasketIdx = "",
                CustomerIdx = "012345678901234567890123",
                ProductIdx = "333l291e810c13729de86333",
                Quantity = 1,
                SalePrice = (decimal)9.99
            };
            var expectedResult = new Result<BasketWithItemResponse>()
            {
                IsSuccessful = true,
                ErrorMessages = new List<ErrorModel>(),
                EntityList = new List<BasketWithItemResponse>()
                {
                    new BasketWithItemResponse()
                    {
                        Id="012345678901234567890123",
                        CreatedDate=DateTime.Now,
                        CustomerIdx="012345678901234567890123",
                        BasketItem= new List<BasketItem>()
                        {
                            new BasketItem()
                            {
                                Id="012345678901234567890123",
                                BasketIdx="012345678901234567890123",
                                ProductIdx="333l291e810c13729de86333",
                                Quantity=1,
                                SalePrice= (decimal)9.99
                            }
                        }
                    }
                },
                EntityCount = 1,
                IsValid = true
            };

            
            mockIBasketService.Setup(x => x.AddOrSetBasketItemAsync(basketWithItemDto)).ReturnsAsync(expectedResult);
            var actualResult = await basketsController.AddOrSetBasketItem(basketWithItemDto);


            var okResult = Assert.IsType<OkObjectResult>(actualResult);
            var resultmodal = Assert.IsType<Result<BasketWithItemResponse>>(okResult.Value);

            Assert.True(resultmodal.IsSuccessful);
            Assert.Empty(resultmodal.ErrorMessages);
            Assert.Equal(expectedResult, resultmodal);
            mockIBasketService.Verify(x=>x.AddOrSetBasketItemAsync(It.IsAny<BasketWithItemDto>()),Times.Once);
        }

        [Theory]
        [InlineData("5ffa5a59024620194d371deb")]
        public async Task GetWithBasketItemsById_IdInValid_ReturnBadRequest(string basketId)
        {
            var expectedResult = new Result<BasketWithItemResponse>()
            {
                IsSuccessful = true,
                ErrorMessages = new List<ErrorModel>(),
                EntityList = new List<BasketWithItemResponse>()
                {
                    new BasketWithItemResponse()
                    {
                        Id="5ffa5a59024620194d371deb",
                        CreatedDate=DateTime.Now,
                        CustomerIdx="054321678901234567890123",
                        BasketItem= new List<BasketItem>()
                        {
                            new BasketItem()
                            {
                                Id="012345678901234567890123",
                                BasketIdx="5ffa5a59024620194d371deb",
                                ProductIdx="333l291e810c13729de86333",
                                Quantity=2,
                                SalePrice= (decimal)19.99
                            }
                        }
                    }
                },
                EntityCount = 1,
                IsValid = true
            };

            mockIBasketService.Setup(x => x.GetByIdBasketAsync(basketId)).ReturnsAsync(expectedResult);
            var actualResult = await basketsController.GetWithBasketItemsById(basketId);


            var okResult = Assert.IsType<OkObjectResult>(actualResult);
            var resultmodal = Assert.IsType<Result<BasketWithItemResponse>>(okResult.Value);
            Assert.True(resultmodal.IsSuccessful);
            Assert.Empty(resultmodal.ErrorMessages);
            Assert.Equal(expectedResult, resultmodal);
            mockIBasketService.Verify(x => x.GetByIdBasketAsync(It.IsAny<string>()), Times.Once);
        }

    }
}
