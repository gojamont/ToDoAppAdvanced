using Moq;
using ToDoAdvanced.Models; // This namespace provides PriorityLevel and ToDoStatus
using ToDoAdvanced.Services.DataService;
using ToDoAdvanced.Services.ToDoManager;

namespace Tests
{
    public class ToDoManagerTests
    {
        private readonly Mock<IDataService> _mockDataService;
        private readonly Mock<IDataReader> _mockFileReader;
        private readonly Mock<IDataSaver> _mockFileSaver;
        private readonly Mock<IDataWriter> _mockDataWriter; // Added
        private readonly Mock<IDataLoader> _mockDataLoader; // Added
        private readonly ToDoManager _toDoManager;
        private readonly string _testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoList.json");

        public ToDoManagerTests()
        {
            _mockFileReader = new Mock<IDataReader>();
            _mockFileSaver = new Mock<IDataSaver>();
            _mockDataService = new Mock<IDataService>();
            _mockDataWriter = new Mock<IDataWriter>(); // Initialize
            _mockDataLoader = new Mock<IDataLoader>();

            _mockDataService.Setup(s => s.FileDataReader).Returns(_mockFileReader.Object);
            _mockDataService.Setup(s => s.FileDataSaver).Returns(_mockFileSaver.Object);
            _mockDataService.Setup(s => s.FileDataWriter).Returns(_mockDataWriter.Object); // Setup
            _mockDataService.Setup(s => s.FileDataLoader).Returns(_mockDataLoader.Object); // Setup


            _mockFileReader.Setup(r => r.ReadDataAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ToDoItem>());
            
            _toDoManager = new ToDoManager(_mockDataService.Object);
        }

        [Fact]
        public void Constructor_NullDataService_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>("dataService", () => new ToDoManager(null!));
        }

    [Fact]
    public async Task Add_AddsItemAndSaves()
    {
        // Arrange
        // Using DateTimeOffset.Now for consistency with ToDoItem's Date property
        var now = DateTimeOffset.Now; 
        var newItem = new ToDoItem("Test Task", "Description", PriorityLevel.Medium, ToDoStatus.NotStarted, now, TimeSpan.Zero);
        var initialItems = new List<ToDoItem>();
        _mockFileReader.Setup(r => r.ReadDataAsync(_testFilePath)).ReturnsAsync(initialItems);

        // Act
        await _toDoManager.Add(newItem);

        // Assert
        _mockFileReader.Verify(r => r.ReadDataAsync(_testFilePath), Times.Once);

        // Verify SaveDataAsync by checking the properties of the saved item
        _mockFileSaver.Verify(s => s.SaveDataAsync(
                _testFilePath,
                It.Is<List<ToDoItem>>(list =>
                        list.Count == 1 &&
                        list[0].Name == newItem.Name &&
                        list[0].Description == newItem.Description &&
                        list[0].Priority == newItem.Priority &&
                        list[0].Status == newItem.Status &&
                        // Comparing DateTimeOffset properties
                        list[0].Date == newItem.Date 
                        // Removed Duration check
                )),
            Times.Once);

        // Assert that _toDoManager.Items contains one item with properties matching newItem.
        Assert.Single(_toDoManager.Items); // Verifies that there is exactly one item in the collection.
        var itemInManager = _toDoManager.Items.First();
        Assert.Equal(newItem.Name, itemInManager.Name);
        Assert.Equal(newItem.Description, itemInManager.Description);
        Assert.Equal(newItem.Priority, itemInManager.Priority);
        Assert.Equal(newItem.Status, itemInManager.Status);
        Assert.Equal(newItem.Date, itemInManager.Date); // Check Date property
    }
        [Fact]
        public async Task Delete_RemovesItemAndSaves()
        {
            // Arrange
            var itemToRemove = new ToDoItem("Test Task", "Description", PriorityLevel.Medium, ToDoStatus.NotStarted, DateTime.Now, TimeSpan.Zero);
            var initialItems = new List<ToDoItem> { itemToRemove };
            _mockFileReader.Setup(r => r.ReadDataAsync(_testFilePath)).ReturnsAsync(initialItems);

            // Act
            await _toDoManager.Delete(itemToRemove);

            // Assert
            _mockFileReader.Verify(r => r.ReadDataAsync(_testFilePath), Times.Once);
            _mockFileSaver.Verify(s => s.SaveDataAsync(_testFilePath, It.Is<List<ToDoItem>>(list => !list.Contains(itemToRemove))), Times.Once);
            Assert.DoesNotContain(itemToRemove, _toDoManager.Items);
        }

        [Fact]
        public async Task Delete_ItemNotExisting_DoesNotThrowAndSaves()
        {
            // Arrange
            var itemToRemove = new ToDoItem("Non Existent Task", "Description", PriorityLevel.Medium, ToDoStatus.NotStarted, DateTime.Now, TimeSpan.Zero);
            var existingItem = new ToDoItem("Existing Task", "Description", PriorityLevel.Medium, ToDoStatus.NotStarted, DateTime.Now, TimeSpan.Zero);
            var initialItems = new List<ToDoItem> { existingItem };
            _mockFileReader.Setup(r => r.ReadDataAsync(_testFilePath)).ReturnsAsync(initialItems);

            // Act
            await _toDoManager.Delete(itemToRemove);

            // Assert
            _mockFileReader.Verify(r => r.ReadDataAsync(_testFilePath), Times.Once);
            _mockFileSaver.Verify(s => s.SaveDataAsync(_testFilePath, It.Is<List<ToDoItem>>(list => list.Count == 1 && list.Contains(existingItem))), Times.Once);
            Assert.Contains(existingItem, _toDoManager.Items);
            Assert.DoesNotContain(itemToRemove, _toDoManager.Items);
        }


        [Fact]
        public async Task ClearAll_ClearsItemsAndSaves()
        {
            // Arrange
            var initialItems = new List<ToDoItem>
            {
                new("Task 1", "Desc 1", PriorityLevel.High, ToDoStatus.NotStarted, DateTime.Now, TimeSpan.Zero),
                new("Task 2", "Desc 2", PriorityLevel.Low, ToDoStatus.InProgress, DateTime.Now, TimeSpan.Zero)
            };
            _mockFileReader.Setup(r => r.ReadDataAsync(_testFilePath)).ReturnsAsync(initialItems);

            // Act
            await _toDoManager.ClearAll();

            // Assert
            _mockFileReader.Verify(r => r.ReadDataAsync(_testFilePath), Times.Once);
            _mockFileSaver.Verify(s => s.SaveDataAsync(_testFilePath, It.Is<List<ToDoItem>>(list => !list.Any())), Times.Once);
            Assert.Empty(_toDoManager.Items);
        }

        [Fact]
        public async Task IsDone_UpdatesItemStatusToCompletedAndSaves()
        {
            // Arrange
            var itemToUpdate = new ToDoItem("Test Task", "Description", PriorityLevel.Medium, ToDoStatus.InProgress, DateTime.Now, TimeSpan.Zero);
            var initialItems = new List<ToDoItem> { itemToUpdate };
            _mockFileReader.Setup(r => r.ReadDataAsync(_testFilePath)).ReturnsAsync(initialItems);

            // Act
            await _toDoManager.IsDone(itemToUpdate);

            // Assert
            _mockFileReader.Verify(r => r.ReadDataAsync(_testFilePath), Times.Once);
            _mockFileSaver.Verify(s => s.SaveDataAsync(_testFilePath, It.Is<List<ToDoItem>>(list => list.First().Status == ToDoStatus.Completed)), Times.Once);
            Assert.Equal(ToDoStatus.Completed, itemToUpdate.Status);
        }

        [Fact]
        public async Task IsDone_ItemNotExisting_DoesNotThrowAndSaves()
        {
            // Arrange
            var itemToUpdate = new ToDoItem("Non Existent Task", "Description", PriorityLevel.Medium, ToDoStatus.InProgress, DateTime.Now, TimeSpan.Zero);
            var existingItem = new ToDoItem("Existing Task", "Description", PriorityLevel.Medium, ToDoStatus.NotStarted, DateTime.Now, TimeSpan.Zero);
            var initialItems = new List<ToDoItem> { existingItem };
            _mockFileReader.Setup(r => r.ReadDataAsync(_testFilePath)).ReturnsAsync(initialItems);

            // Act
            await _toDoManager.IsDone(itemToUpdate);

            // Assert
            _mockFileReader.Verify(r => r.ReadDataAsync(_testFilePath), Times.Once);
            // Verify SaveDataAsync is NOT called because the item to update was not found
            _mockFileSaver.Verify(s => s.SaveDataAsync(It.IsAny<string>(), It.IsAny<List<ToDoItem>>()), Times.Never());
            Assert.Equal(ToDoStatus.NotStarted, existingItem.Status); // Original item unchanged
        }

        [Fact]
        public async Task InProgress_UpdatesItemStatusToInProgressAndSaves()
        {
            // Arrange
            var itemToUpdate = new ToDoItem("Test Task", "Description", PriorityLevel.Medium, ToDoStatus.NotStarted, DateTime.Now, TimeSpan.Zero);
            var initialItems = new List<ToDoItem> { itemToUpdate };
            _mockFileReader.Setup(r => r.ReadDataAsync(_testFilePath)).ReturnsAsync(initialItems);

            // Act
            await _toDoManager.InProgress(itemToUpdate);

            // Assert
            _mockFileReader.Verify(r => r.ReadDataAsync(_testFilePath), Times.Once);
            _mockFileSaver.Verify(s => s.SaveDataAsync(_testFilePath, It.Is<List<ToDoItem>>(list => list.First().Status == ToDoStatus.InProgress)), Times.Once);
            Assert.Equal(ToDoStatus.InProgress, itemToUpdate.Status);
        }

        [Fact]
        public async Task InProgress_ItemNotExisting_DoesNotThrowAndSaves()
        {
            // Arrange
            var itemToUpdate = new ToDoItem("Non Existent Task", "Description", PriorityLevel.Medium, ToDoStatus.NotStarted, DateTime.Now, TimeSpan.Zero);
            var existingItem = new ToDoItem("Existing Task", "Description", PriorityLevel.Medium, ToDoStatus.Completed, DateTime.Now, TimeSpan.Zero);
            var initialItems = new List<ToDoItem> { existingItem };
            _mockFileReader.Setup(r => r.ReadDataAsync(_testFilePath)).ReturnsAsync(initialItems);

            // Act
            await _toDoManager.InProgress(itemToUpdate);

            // Assert
            _mockFileReader.Verify(r => r.ReadDataAsync(_testFilePath), Times.Once);
            // Verify SaveDataAsync is NOT called because the item to update was not found
            _mockFileSaver.Verify(s => s.SaveDataAsync(It.IsAny<string>(), It.IsAny<List<ToDoItem>>()), Times.Never());
            Assert.Equal(ToDoStatus.Completed, existingItem.Status); // Original item unchanged
        }
        
        [Fact]
        public async Task Delete_RemovesItemAndSaves_SimpleCheck()
        {
            // Arrange
            var itemToRemove = new ToDoItem("Test Task", "Description", PriorityLevel.Medium, ToDoStatus.NotStarted, DateTime.Now, TimeSpan.Zero);
            var initialItems = new List<ToDoItem> { itemToRemove };
            _mockFileReader.Setup(r => r.ReadDataAsync(_testFilePath)).ReturnsAsync(initialItems);

            // Act
            await _toDoManager.Delete(itemToRemove);

            // Assert
            _mockFileReader.Verify(r => r.ReadDataAsync(_testFilePath), Times.Once);
            _mockFileSaver.Verify(s => s.SaveDataAsync(_testFilePath, It.Is<List<ToDoItem>>(list => !list.Contains(itemToRemove))), Times.Once);
            Assert.DoesNotContain(itemToRemove, _toDoManager.Items);
        }
        
    }
}