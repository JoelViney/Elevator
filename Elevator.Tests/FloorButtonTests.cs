using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elevator
{
    /// <summary>
    /// Tests the scenarios where people on each floor can request an elevator.
    /// </summary>
    [TestClass]
    public class FloorButtonTests
    {        
        // The elevator is on level 1. 
        // Someone on the 8th Floor presses a button.
        [TestMethod]
        public void CallElevatorBelow()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            elevator.PressFloorButton(8, Direction.Down);
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 1, 8 }, elevator.FloorHistory);
        }

        // The elevator is on the 8th floor. 
        // Someone on level 1 presses the button.
        [TestMethod]
        public void CallElevatorAbove()
        {
            // Arrange
            var elevator = new Elevator(8);

            // Act
            elevator.PressFloorButton(1, Direction.Up);
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 8, 1 }, elevator.FloorHistory);
        }

        [TestMethod]
        public void IgnoreMultipleFloorPresses()
        {
            // Arrange
            var elevator = new Elevator(1);

            elevator.PressFloorButton(4, Direction.Down);
            elevator.PressFloorButton(4, Direction.Down);
            elevator.PressFloorButton(4, Direction.Down);
            elevator.PressFloorButton(4, Direction.Down);
            elevator.Move();

            // Act
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 1, 4 }, elevator.FloorHistory);
        }

        // Someone on level 1 presses the button for level 8.
        // Someone on the 4th floor presses the button to go up.
        // The elevator should pick them up on the way through.
        [TestMethod]
        public void PickUpWhilePassingFloor()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            elevator.PressButton(8);
            elevator.PressFloorButton(4, Direction.Up);
            elevator.Move();
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 1, 4, 8 }, elevator.FloorHistory);
        }

        // Someone on level 1 presses the button for level 8.
        // Someone on the 4th floor presses the button to go down.
        // The elevator shouldn't pick them up because they want to go in the opposite direction.
        [TestMethod]
        public void DontPickForTheWrongDirection()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            elevator.PressButton(8);
            elevator.PressFloorButton(4, Direction.Down);
            elevator.Move();
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 1, 8, 4 }, elevator.FloorHistory);
        }
    }
}
