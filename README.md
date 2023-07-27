# UnityHelicopter

## A Unity helicopter controller

### Design notes

- Inputs are exposed in the editor through the HeliControlValues behaviour, allowing automation from the Timeline or an animation. 

- The HeliInput behaviour routes input through heliControlValues and must be disabled is another input source is used.

### Controls

- W/S: Cyclic forward/back
- A/D: Cyclic left/right
- UP/DOWN: Throttle
- LEFT/RIGHT: Tail rotor
- F: Change cammera view (rear, pilot, left, right)

### YouTube video of me flying around all four L4D2 Dead Center levels joined together
[![YouTube Video](https://img.youtube.com/vi/C1UqkVcUXvU/0.jpg)](https://youtu.be/C1UqkVcUXvU)
