using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Elevator
{
    [TestClass]
    public class ElevatorTests
    {
        [TestMethod]
        public void MoveUp()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            elevator.PressButton(4);

            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 1, 4 }, elevator.History);
        }

        [TestMethod]
        public void MoveDown()
        {
            // Arrange
            var elevator = new Elevator(4);

            // Act
            elevator.PressButton(1);

            elevator.Move();

            // Assert
            CollectionAssert.AreEqual(new List<int>() { 4, 1 }, elevator.History);
        }

        /*
         * In a building with many floors, the computer has to have some sort of strategy to keep 
         * the cars running as efficiently as possible. In older systems, the strategy is to avoid 
         * reversing the elevator's direction. That is, an elevator car will keep moving up as long 
         * as there are people on the floors above that want to go up. The car will only answer 
         * "down calls" after it has taken care of all the "up calls." But once it starts down, it 
         * won't pick up anybody who wants to go up until there are no more down calls on lower 
         * floors. 
         * This program does a pretty good job of getting everybody to their floor as fast as 
         * possible, but it is highly inflexible.
         * 
         * The Elevator is at level 1.
         * John gets on and presses the button 4
         * Jane gets on and presses the button 6
         * The lift moves to its first destination 
         * Bill gets on and presses the button 2.
         * Mark gets on and presses the button 5.
        */
        [TestMethod]
        public void ElevatorTest()
        {
            // Arrange
            var elevator = new Elevator();

            // Act
            elevator.PressButton(4);
            elevator.PressButton(6);

            elevator.Move();

            elevator.PressButton(2);
            elevator.PressButton(5);

            while (elevator.HasSomewhereToGo())
            {
                elevator.Move();
            }

            // Assert
            CollectionAssert.AreEqual(new List<int>() {1, 4, 5, 6, 2}, elevator.History);
        }
    }
}
