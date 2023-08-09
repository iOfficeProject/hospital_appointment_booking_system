﻿using Xunit;
using Hospital_Appointment_Booking_System.Controllers;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;

public class SlotControllerTests
{
    private readonly SlotController _slotController;
    private readonly ISlotRepository _fakeSlotRepository;

    public SlotControllerTests()
    {
        _fakeSlotRepository = A.Fake<ISlotRepository>();
        _slotController = new SlotController(_fakeSlotRepository);
    }

    [Fact]
    public async Task GetAllSlots_SlotsExist_ReturnsOkResultWithSlots()
    {
        // Arrange
        var slots = new List<SlotDTO>
        {
            new SlotDTO { SlotId = 1, SlotDate = DateTime.Now.Date, SlotStartTime = DateTime.Now, SlotEndTime = DateTime.Now.AddHours(1) },
            new SlotDTO { SlotId = 2, SlotDate = DateTime.Now.Date.AddDays(1), SlotStartTime = DateTime.Now.AddHours(2), SlotEndTime = DateTime.Now.AddHours(3) }
        };

        A.CallTo(() => _fakeSlotRepository.GetAllSlots()).Returns(slots);

        // Act
        var result = await _slotController.GetAllSlots();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var slotList = Assert.IsType<List<SlotDTO>>(okResult.Value);
        Assert.Equal(slots.Count, slotList.Count);
    }

    [Fact]
    public async Task GetAllSlots_NoSlotsExist_ReturnsOkResultWithEmptyList()
    {
        // Arrange
        A.CallTo(() => _fakeSlotRepository.GetAllSlots()).Returns(new List<SlotDTO>());

        // Act
        var result = await _slotController.GetAllSlots();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var slotList = Assert.IsType<List<SlotDTO>>(okResult.Value);
        Assert.Empty(slotList);
    }

    [Fact]
    public async Task GetSlotById_ExistingId_ReturnsOkResultWithSlot()
    {
        // Arrange
        var slotId = 1;
        var slot = new SlotDTO { SlotId = slotId, SlotDate = DateTime.Now.Date, SlotStartTime = DateTime.Now, SlotEndTime = DateTime.Now.AddHours(1) };

        A.CallTo(() => _fakeSlotRepository.GetSlotById(slotId)).Returns(slot);

        // Act
        var result = await _slotController.GetSlotById(slotId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var returnedSlot = Assert.IsType<SlotDTO>(okResult.Value);
        Assert.Equal(slot.SlotId, returnedSlot.SlotId);
    }

    [Fact]
    public async Task GetSlotById_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        var slotId = 1;
        A.CallTo(() => _fakeSlotRepository.GetSlotById(slotId)).Returns(Task.FromResult<SlotDTO>(null));

        // Act
        var result = await _slotController.GetSlotById(slotId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task AddSlot_ValidSlot_ReturnsOkResult()
    {
        // Arrange
        var slotDto = new SlotDTO
        {
            SlotDate = DateTime.Now.Date.AddDays(2),
            SlotStartTime = DateTime.Now.AddHours(4),
            SlotEndTime = DateTime.Now.AddHours(5)
        };

        // Act
        var result = await _slotController.AddSlot(slotDto);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        A.CallTo(() => _fakeSlotRepository.AddSlot(slotDto)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateSlot_ExistingSlotId_ReturnsOkResult()
    {
        // Arrange
        var slotId = 1;
        var slotDto = new SlotDTO
        {
            SlotId = slotId,
            SlotDate = DateTime.Now.Date.AddDays(3),
            SlotStartTime = DateTime.Now.AddHours(6),
            SlotEndTime = DateTime.Now.AddHours(7)
        };

        A.CallTo(() => _fakeSlotRepository.UpdateSlot(slotDto));

        // Act
        var result = await _slotController.UpdateSlot(slotId, slotDto);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        A.CallTo(() => _fakeSlotRepository.UpdateSlot(slotDto)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteSlot_ExistingSlotId_ReturnsOkResult()
    {
        // Arrange
        var slotId = 1;

        // Act
        var result = await _slotController.DeleteSlot(slotId);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        A.CallTo(() => _fakeSlotRepository.DeleteSlot(slotId)).MustHaveHappenedOnceExactly();
    }

}
