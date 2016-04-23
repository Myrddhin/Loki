using Loki.UI.Commands;

using Moq;

using Xunit;

namespace Loki.Core.Tests.UI
{
    public class ApplicationCommandsTest
    {
        public ApplicationCommandsTest()
        {
            MockService = new Mock<ICommandComponent>();

            Commands = new ApplicationCommands(MockService.Object);
            var mockCommand = new Mock<ICommand>();
            MockService.Setup(x => x.GetCommand(It.IsAny<string>())).Returns(mockCommand.Object);
        }

        public Mock<ICommandComponent> MockService { get; set; }

        public ApplicationCommands Commands { get; set; }

        [Fact]
        public void Export()
        {
            Assert.NotNull(Commands.Export);
            MockService.Verify(x => x.GetCommand(ApplicationCommands.Names.EXPORT));
        }

        [Fact]
        public void Save()
        {
            Assert.NotNull(Commands.Save);
            MockService.Verify(x => x.GetCommand(ApplicationCommands.Names.SAVE));
        }

        [Fact]
        public void Open()
        {
            Assert.NotNull(Commands.Open);
            MockService.Verify(x => x.GetCommand(ApplicationCommands.Names.OPEN));
        }

        [Fact]
        public void Refresh()
        {
            Assert.NotNull(Commands.Refresh);
            MockService.Verify(x => x.GetCommand(ApplicationCommands.Names.REFRESH));
        }

        [Fact]
        public void UpdateStatus()
        {
            Assert.NotNull(Commands.UpdateStatus);
            MockService.Verify(x => x.GetCommand(ApplicationCommands.Names.UPDATESTATUS));
        }

        [Fact]
        public void Search()
        {
            Assert.NotNull(Commands.Search);
            MockService.Verify(x => x.GetCommand(ApplicationCommands.Names.SEARCH));
        }
    }
}