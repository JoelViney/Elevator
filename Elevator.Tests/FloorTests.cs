using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Elevator
{

    [TestClass]
    public class FloorTests
    {
        [TestMethod]
        public void HigherFloorPressed()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            elevator.PressFloorButton(4, Direction.Down);
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 1, 4 }, elevator.FloorHistory);
        }

        [TestMethod]
        public void LowerFloorPressed()
        {
            // Arrange
            var elevator = new Elevator(5);

            // Act
            elevator.PressFloorButton(1, Direction.Up);
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 5, 1 }, elevator.FloorHistory);
        }

        [TestMethod]
        public void PickUpWhilePassingFloor()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            elevator.PressFloorButton(8, Direction.None);
            elevator.PressFloorButton(4, Direction.Up);
            elevator.Move();
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 1, 4, 8 }, elevator.FloorHistory);

        }






    }
}
