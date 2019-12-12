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

        public List<int> PressedButtons { get; set; } 

        public List<int> PressedButtonsHistory { get; }

        #region Constructors...

        public Elevator(int floor = 1)
        {
            Debug.WriteLine("Elevator Created");
            this.Direction = Direction.None;
            this.PressedButtonsHistory = new List<int>();
            this.PressedButtons = new List<int>();
            this.SetFloor(floor);
        }

        #endregion

        public void SetFloor(int floor)
        {
            Debug.WriteLine($"Moved to floor {floor}");

            this.Floor = floor;
            this.PressedButtonsHistory.Add(floor);
        }

        public void PressButton(int number)
        {
            if (this.Floor == number)
            {
                return; // KEEP THE DOOR OPEN!
            }

            if (this.PressedButtons.Contains(number))
            {
                return; // Stop pressing the same button.
            }

            this.PressedButtons.Add(number);
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
                if (this.Floor > this.PressedButtons[0])
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
                if (this.PressedButtons.Where(x => x > this.Floor).Count() == 0)
                {
                    this.Direction = Direction.Down;                
                }
            }
            else if (this.Direction == Direction.Down)
            {
                if (this.PressedButtons.Where(x => x < this.Floor).Count() == 0)
                {
                    this.Direction = Direction.Up;
                }
            }

            // Find the next floor 
            int? nextFloor = null;
            if (this.Direction == Direction.Up)
            {
                nextFloor = this.PressedButtons.Where(x => x > this.Floor).OrderBy(x => x).FirstOrDefault();
            }

            if (this.Direction == Direction.Down)
            {
                nextFloor = this.PressedButtons.Where(x => x < this.Floor).OrderByDescending(x => x).FirstOrDefault();
            }

            this.PressedButtons.Remove(nextFloor.Value);
            this.SetFloor(nextFloor.Value);
        }
    }
}
