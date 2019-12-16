using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Elevator
{
    public enum Direction
    {
        None,
        Up,
        Down
    }

    /// <summary>
    /// A button press is from inside the elevator which has a direction of none.
    /// Or from a floor outside the eleveator which has a direction indicating up or down.
    /// </summary>
    public class ButtonPress
    { 
        public int Floor { get; }

        public Direction Direction { get; }

        public ButtonPress(int floor)
        {
            this.Floor = floor;
            this.Direction = Direction.None;
        }

        public ButtonPress(int floor, Direction direction)
        {
            this.Floor = floor;
            this.Direction = direction;
        }
    }

    /*
     * In a building with many floors, the computer has to have some sort of strategy to keep 
     * the cars running as efficiently as possible. In older systems, the strategy is to avoid 
     * reversing the elevator's direction. That is, an elevator car will keep moving up as long 
     * as there are people on the floors above that want to go up. The car will only answer 
     * "down calls" after it has taken care of all the "up calls." But once it starts down, it 
     * won't pick up anybody who wants to go up until there are no more down calls on lower 
     *
     * https://science.howstuffworks.com/transport/engines-equipment/elevator7.htm
    */
    public class Elevator
    {
        public int Floor { get; private set; }
        public Direction Direction { get; private set; }

        public List<ButtonPress> PressedButtons { get; } 

        public List<int> FloorHistory { get; }

        #region Constructors...

        public Elevator(int floor = 1)
        {
            Debug.WriteLine("Elevator Created");
            this.Direction = Direction.None;
            this.FloorHistory = new List<int>();
            this.PressedButtons = new List<ButtonPress>();
            this.SetFloor(floor);
        }

        #endregion

        public void SetFloor(int floor)
        {
            Debug.WriteLine($"Moved to floor {floor}");

            this.Floor = floor;
            this.FloorHistory.Add(floor);
        }

        public void PressButton(int floor)
        {
            if (this.Floor == floor)
            {
                return; // KEEP THE DOOR OPEN!
            }

            if (this.PressedButtons.Any(x => x.Floor == floor && x.Direction == Direction.None))
            {
                return; // Stop pressing the same button.
            }

            this.PressedButtons.Add(new ButtonPress(floor));
        }

        public void PressFloorButton(int floor, Direction direction)
        {
            if (this.Floor == floor)
            {
                return; // KEEP THE DOOR OPEN
            }

            if (this.PressedButtons.Any(x => x.Floor == floor && x.Direction == direction))
            {
                return; // Stop pressing the same button.
            }

            this.PressedButtons.Add(new ButtonPress(floor, direction));
        }

        public bool HasSomewhereToGo()
        {
            return (this.PressedButtons.Count > 0);
        }

        public void Move()
        {
            if (!this.HasSomewhereToGo())
            {
                return;
            }

            // Work out the start direction
            if (this.Direction == Direction.None)
            {
                if (this.Floor > this.PressedButtons[0].Floor)
                {
                    this.Direction = Direction.Down;
                }
                else
                {
                    this.Direction = Direction.Up;
                }
            } 
            else if (this.Direction == Direction.Up)
            {
                if (!this.PressedButtons.Any(x => x.Floor > this.Floor && (x.Direction == Direction.Up || x.Direction == Direction.None)))
                {
                    this.Direction = Direction.Down;                
                }
            }
            else if (this.Direction == Direction.Down)
            {
                if (!this.PressedButtons.Any(x => x.Floor < this.Floor && (x.Direction == Direction.Down || x.Direction == Direction.None)))
                {
                    this.Direction = Direction.Up;
                }
            }

            // Find the next floor 
            ButtonPress nextButton = null;
            if (this.Direction == Direction.Up)
            {
                nextButton = this.PressedButtons.Where(x => x.Floor > this.Floor && (x.Direction == Direction.Up || x.Direction == Direction.None)).OrderBy(x => x.Floor).FirstOrDefault();
            }

            if (this.Direction == Direction.Down)
            {
                nextButton = this.PressedButtons.Where(x => x.Floor < this.Floor && (x.Direction == Direction.Down || x.Direction == Direction.None)).OrderByDescending(x => x.Floor).FirstOrDefault();
            }

            this.PressedButtons.Remove(nextButton);
            this.SetFloor(nextButton.Floor);
        }
    }
}
