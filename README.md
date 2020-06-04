# Elevator
 
The elevator algorithm, a simple but elegant algorithm by which a single elevator can decide where to stop, is summarised as follows:

* As long as there’s someone inside or ahead of the elevator who wants to go in the current direction, keep heading in that direction.
* Once the elevator has exhausted the requests in its current direction, switch directions if there’s a request in the other direction. Otherwise, stop and wait for a call.

The implementation used TDD. It was written for fun on a Friday afternoon with a beer in hand. 
