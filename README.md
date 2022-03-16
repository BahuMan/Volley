# Volley
2D physics based volleyball game against naive computer

for educational purposes.
This started as a simple, 3 script game, but the "volleyball.cs" script got complicated quickly in order to enforce points, rules, serves.
And the PC script is getting complicated too in order for the "AI" player to become a somewhat enjoyable opponent.

Currently, the PC jums whenever the ball is near its head, and when the ball threatens to fall, the PC does a linear extrapolation about where the ball might fall. It will then run to that place. Since the playing field isn't very big, linear extrapolation does not differ significantly from the actual parabola trajectory.

![screenshot](info/Screenshot.jpg?raw=true)
