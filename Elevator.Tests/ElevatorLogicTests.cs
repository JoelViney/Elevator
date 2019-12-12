using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Elevator
{
    [TestClass]
    public class ElevatorLogicTests
    {
        [TestMethod]
        public void ElevatorWillMoveUp()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            elevator.PressButton(4);
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 1, 4 }, elevator.PressedButtonsHistory);
        }

        [TestMethod]
        public void ElevatorWillMoveDown()
        {
            // Arrange
            var elevator = new Elevator(4);

            // Act
            elevator.PressButton(1);
            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 4, 1 }, elevator.PressedButtonsHistory);
        }

        /*
         * The Elevator is at level 1.
         * John gets on and presses the button 4
         * The lift moves to its first destination.
         * Bill gets on and presses the button 2.
         * Mark gets on and presses the button 8.
         * 
         * The elevator should go all the way up to the 6th floor, then change direction and come
         * back down.
        */
        [TestMethod]
        public void ElevatorWillMaintainDirection()
        {
            // Arrange
            var elevator = new Elevator();
            elevator.PressButton(4);

            elevator.Move();

            elevator.PressButton(2);
            elevator.PressButton(8);
            elevator.Move();

            // Act
            elevator.Move(); // This move it should be heading down.

            // Assert
            Assert.AreEqual(Direction.Down, elevator.Direction);
            CollectionAssert.AreEqual(new List<int>() {1, 4, 8, 2}, elevator.PressedButtonsHistory);
        }
    }
}
