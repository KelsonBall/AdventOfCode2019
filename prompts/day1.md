# --- Day 1: The Tyranny of the Rocket Equation ---

## Word Problem Synopsis 

The world problem refers to a famous formula from Rocket Science called [The Rocket Equation](https://en.wikipedia.org/wiki/Tsiolkovsky_rocket_equation), which relates 3 parameters of a rocket (payload mass, fuel mass, and engine exhaust velocity) together to determine how much the rocket can change its velocity ("delta-v").

In this puzzle:
 * the "payload mass" is given as inputs (named "modules")
 * the "fuel mass" is what we must solve for
 * the "exhaust velocity" can be ignored
 * the "delta-v" required is known by Santa's Elves

## Part 1 

You are given a formula for converting from payload mass to fuel mass required to achieve the "delta-v" required to go on our journey. 

That formula is:

```
fuel_mass = (module_mass / 3) - 2
```

### Test Cases

 * `0` maps to `0`
 * `12` maps to `2`
 * `14` maps to `2`
 * `1969` maps to `654`
 * `100756` maps to `33583`

 
### Task 

 Your puzzle input is a text file containing one modules mass on each line. 
 Calculate the sum of fuel masses required for all of the modules.

## Part 2 

 Fuel has mass too, and added fuel effectively becomes new "payload" mass. (Hence the "tyranny" in the puzzle title).

 To determine how much fuel is truely required, you must also add the fuel required by the fuel you add.

 <details>
    <summary>Hint - CS concepts to apply</summary> 

    Part 2 of this puzzle is neatly solved by applying [recursion](https://www.topcoder.com/community/competitive-programming/tutorials/an-introduction-to-recursion-part-1/)
 </details>


### Test Cases

 * `0` maps to `0`
 * `14` maps to `2`
 * `1969` maps to `966`
 * `100756` maps to `50346`

### Task

 Given the same inputs, calculate the total fuel required (including the fuel required to push the fuel you add).

## More Info

 Scott Manley video demonstrating "delta-v" in Kerbal Space Program: https://youtu.be/zLitRxZMsSc

