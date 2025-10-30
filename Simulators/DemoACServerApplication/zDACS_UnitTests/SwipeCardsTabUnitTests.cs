using DemoServerApplication.ACSystem;
using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace zDACS_UnitTests
{
    [TestClass]
    public class SwipeCardsTabUnitTests
    {
        private DoorManager _doorManager;
        private ConfigurationManager _configurationManager;
        private SwipeCardsTabViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _doorManager = DoorManager.Instance;
            _configurationManager = ConfigurationManager.Instance;

            _viewModel = new SwipeCardsTabViewModel(null);
        }

        [TestMethod]
        public void LockAllDoorsCommand_TestMethod()
        {
            // Arrange

            // Act
            _viewModel.LockAllDoorsCommand.Execute(null);

            // Assert
            foreach (var door in _configurationManager.Doors)
                Assert.IsFalse(_doorManager.GetDoorStatus(door.Id).IsLocked);
        }

        [TestMethod]
        public void UnlockAllDoorsCommand_TestMethod()
        {
            // Arrange

            // Act
            _viewModel.UnlockAllDoorsCommand.Execute(null);

            // Assert
            foreach (var door in _configurationManager.Doors)
                Assert.IsTrue(_doorManager.GetDoorStatus(door.Id).IsLocked);
        }

        [TestCleanup]
        public void Teardown()
        {
            _doorManager = null;
            _configurationManager = null;
            _viewModel = null;
        }
    }
}
