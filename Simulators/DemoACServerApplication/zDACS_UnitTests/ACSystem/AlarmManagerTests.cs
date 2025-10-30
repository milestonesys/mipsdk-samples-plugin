using System;
using System.Collections.Generic;
using DemoACServerApplication;
using DemoServerApplication.ACSystem;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace zDACS_UnitTests.ACSystem
{
    [TestClass]
    public class AlarmManagerTests
    {
        private IFixture _fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public void ClearPowerFailureAlarmOnDoor_ShouldAddClearAlarmObjectToQueue()
        {
            // Arrange
            var objectUnderTest = _fixture.Create<AlarmManager>();
            var doorId = _fixture.Create<Guid>();

            // Act
            objectUnderTest.ClearPowerFailureAlarmOnDoor(doorId);

            // Assert
            IEnumerable<ClearAlarmCommand> clearAlarmCommands = objectUnderTest.GetClearAlarmCommands();
            clearAlarmCommands.Should().Contain(c => c.DoorId == doorId && c.EventTypeId == EventManager.EventTypeDoorControllerPowerFailure.Id);
        }

        [TestMethod]
        public void ClearTamperAlarmOnDoor_ShouldAddClearAlarmObjectToQueue()
        {
            // Arrange
            var objectUnderTest = _fixture.Create<AlarmManager>();
            var doorId = _fixture.Create<Guid>();

            // Act
            objectUnderTest.ClearTamperAlarmOnDoor(doorId);

            // Assert
            IEnumerable<ClearAlarmCommand> clearAlarmCommands = objectUnderTest.GetClearAlarmCommands();
            clearAlarmCommands.Should().Contain(c => c.DoorId == doorId && c.EventTypeId == EventManager.EventTypeDoorControllerTampering.Id);
        }

        [TestMethod]
        public void ClearForcedOpenAlarmOnDoor_ShouldAddClearAlarmObjectToQueue()
        {
            // Arrange
            var objectUnderTest = _fixture.Create<AlarmManager>();
            var doorId = _fixture.Create<Guid>();

            // Act
            objectUnderTest.ClearForcedOpenAlarmOnDoor(doorId);

            // Assert
            IEnumerable<ClearAlarmCommand> clearAlarmCommands = objectUnderTest.GetClearAlarmCommands();
            clearAlarmCommands.Should().Contain(c => c.DoorId == doorId && c.EventTypeId == EventManager.EventTypeDoorForcedOpen.Id);
        }
    }
}