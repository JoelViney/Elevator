using System.Diagnostics;

namespace Elevator
{
    public enum Direction
    {
        None,
        Up,
        Down
    }

    /// <summary>
    /// A button press is from inside the elevator which has a direction of none (but it does specify the destination floor).
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

    public class Elevator
    {
        public int Floor { get; private set; }
        public Direction Direction { get; private set; }

        public List<ButtonPress> PressedButtons { get; } 

        public List<int> FloorHistory { get; }

        #region Constructors...

        /// <param name="floor">This is the floor that the eleveator starts at when initialised.</param>
        public Elevator(int floor = 1)
        {
            Debug.WriteLine("Elevator Created");
            this.Direction = Direction.None;
            this.FloorHistory = [];
            this.PressedButtons = [];
            this.SetFloor(floor);
        }

        #endregion

        public void SetFloor(int floor)
        {
            Debug.WriteLine($"Moved to floor {floor}");

            this.Floor = floor;
            this.FloorHistory.Add(floor);
        }

        /// <summary>
        /// Someone presses a button in the elevator.
        /// </summary>
        public void PressButton(int floor)
        {
            if (this.Floor == floor)
            {
                return; // Hold the door
            }

            if (this.PressedButtons.Any(x => x.Floor == floor && x.Direction == Direction.None))
            {
                return; // Stop pressing the same button.
            }

            this.PressedButtons.Add(new ButtonPress(floor));
        }


        /// <summary>
        /// Someone presses a button on one of the floors.
        /// </summary>
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

        private bool HasSomewhereToGo()
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
            ButtonPress? nextButton = null;
            if (this.Direction == Direction.Up)
            {
                nextButton = this.PressedButtons.Where(x => x.Floor > this.Floor && (x.Direction == Direction.Up || x.Direction == Direction.None)).OrderBy(x => x.Floor).FirstOrDefault();

                // Someone has requested the elevator and it has to go the opposite direction
                nextButton ??= this.PressedButtons.First();
            }
            else // if (this.Direction == Direction.Down)
            {
                nextButton = this.PressedButtons.Where(x => x.Floor < this.Floor && (x.Direction == Direction.Down || x.Direction == Direction.None)).OrderByDescending(x => x.Floor).FirstOrDefault();

                // Someone has requested the elevator and it has to go the opposite direction
                nextButton ??= this.PressedButtons.First();
            }

            this.PressedButtons.Remove(nextButton);
            this.SetFloor(nextButton.Floor);
        }
    }
}
