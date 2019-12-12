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

    public class Elevator
    {
        public List<int> History { get; }
        public int Floor { get; private set; }
        public Direction Direction { get; private set; }

        public List<int> RequestedFloors { get; set; }

        #region Constructors...

        public Elevator(int floor = 1)
        {
            Debug.WriteLine("Elevator Created");
            this.Direction = Direction.None;
            this.History = new List<int>();
            this.RequestedFloors = new List<int>();
            this.SetFloor(floor);
        }

        #endregion

        public void SetFloor(int floor)
        {
            Debug.WriteLine($"Moved to floor {floor}");

            this.Floor = floor;
            this.History.Add(floor);
        }

        public void PressButton(int number)
        {
            if (this.Floor == number)
            {
                return; // KEEP THE DOOR OPEN!
            }

            if (this.RequestedFloors.Contains(number))
            {
                return; // Stop pressing the same button.
            }

            this.RequestedFloors.Add(number);
        }

        public bool HasSomewhereToGo()
        {
            return (this.RequestedFloors.Count > 0);
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
                if (this.Floor > this.RequestedFloors[0])
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
                if (this.RequestedFloors.Where(x => x > this.Floor).Count() == 0)
                {
                    this.Direction = Direction.Down;                
                }
            }
            else if (this.Direction == Direction.Down)
            {
                if (this.RequestedFloors.Where(x => x < this.Floor).Count() == 0)
                {
                    this.Direction = Direction.Up;
                }
            }

            // Find the next floor 
            int? nextFloor = null;
            if (this.Direction == Direction.Up)
            {
                nextFloor = this.RequestedFloors.Where(x => x > this.Floor).OrderBy(x => x).FirstOrDefault();
            }

            if (this.Direction == Direction.Down)
            {
                nextFloor = this.RequestedFloors.Where(x => x < this.Floor).OrderByDescending(x => x).FirstOrDefault();
            }

            this.RequestedFloors.Remove(nextFloor.Value);
            this.SetFloor(nextFloor.Value);
        }
    }
}
