using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    [TestMethod]
    public void ReserveSeatSimple()
    {
        Seat seat = new Seat() 
        { 
            Id = 1, 
            Number = 1 
        };

        var seatsServiceMock = new Mock<SeatsService>();
        seatsServiceMock.Setup(s => s.ReserveSeat("1", It.IsAny<int>())).Returns(seat);

        var seatsControllerMock = new Mock<SeatsController>(seatsServiceMock.Object) { CallBase = true };
        seatsControllerMock.Setup(u => u.UserId).Returns("1");

        var actionResult = seatsControllerMock.Object.ReserveSeat(seat.Number);
        var result = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(seat, result.Value);
    }

    [TestMethod]
    public void ReserveSeatDejaReservee()
    {
        Seat seat = new Seat()
        {
            Id = 1,
            Number = 1
        };

        var seatsServiceMock = new Mock<SeatsService>();
        seatsServiceMock.Setup(s => s.ReserveSeat("1", It.IsAny<int>())).Throws(new SeatAlreadyTakenException());

        var seatsControllerMock = new Mock<SeatsController>(seatsServiceMock.Object) { CallBase = true };
        seatsControllerMock.Setup(u => u.UserId).Returns("1");

        var actionResult = seatsControllerMock.Object.ReserveSeat(seat.Number);
        var result = actionResult.Result as UnauthorizedResult;

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveSeatHorsPortee()
    {
        Seat seat = new Seat()
        {
            Id = 1,
            Number = 1
        };

        var seatsServiceMock = new Mock<SeatsService>();
        seatsServiceMock.Setup(s => s.ReserveSeat("1", It.IsAny<int>())).Throws(new SeatOutOfBoundsException());

        var seatsControllerMock = new Mock<SeatsController>(seatsServiceMock.Object) { CallBase = true };
        seatsControllerMock.Setup(u => u.UserId).Returns("1");

        var actionResult = seatsControllerMock.Object.ReserveSeat(seat.Number);
        var result = actionResult.Result as NotFoundObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual("Could not find " + seat.Number, result.Value);
    }

    [TestMethod]
    public void ReserveSeatUtilisateurDejaPlace()
    {
        Seat seat = new Seat()
        {
            Id = 1,
            Number = 1
        };

        var seatsServiceMock = new Mock<SeatsService>();
        seatsServiceMock.Setup(s => s.ReserveSeat("1", It.IsAny<int>())).Throws(new UserAlreadySeatedException());

        var seatsControllerMock = new Mock<SeatsController>(seatsServiceMock.Object) { CallBase = true };
        seatsControllerMock.Setup(u => u.UserId).Returns("1");

        var actionResult = seatsControllerMock.Object.ReserveSeat(seat.Number);
        var result = actionResult.Result as BadRequestResult;

        Assert.IsNotNull(result);
    }
}
