# --- Day 2: 1202 Program Alarm ---

## Word Problem Synopsis 

For puzzle 2 you must program an [emulator](https://www.merriam-webster.com/dictionary/emulator) of a simple computer. The computer in the word problem is a reference to the Apollo Guidence Computer. 

To solve the problem the elves instruct you to make a replacement computer from parts available to you. 

## Part 1 

You must write a program that takes in a list of numbers as a program. Starting at the beginning of the program, interpret the value as an instruction and do what the instruction says. After the instruction is completed, find the value after the previous instruction and all of its parameters and interpret it as the next instruction. Continue until a `stop` instruction is encountered.

The computer has 3 "instructions" that describe manipulations of the program memory. 
Each of these instructions has 0 or more parameters stored in program memory in the spaces immediately after the instruction code.

 * `add` - adds 2 values together
    * instruction code: 1
    * parameter 1: location in memory to get first value from
    * parameter 2: location in memory to get second value from
    * parameter 3: location in memory to write the result of the computation
* `mult` - multiplies 2 values together
    * instruction code: 2
    * parameter 1: location in memory to get first value from
    * parameter 2: location in memory to get second value from
    * parameter 3: location in memory to write the result of the computation
* `stop` - indicates the program has finished execution
    * instruction code: 99
    * has no parameters

In the example program `1,9,10,3,2,3,11,0,99,30,40,50`:
 * The `1` in position 0 indicates to start with an add operation
 * The `9` in position 1 says to use the value in position 9, which is `30`     
 * The `10` in position 2 says to use the value in position 10, which is `40`
 * The `3` in position 3 says to write the result of the add to position 3
 * The `2` in position 4 indicates the next instruction is a multiplication
 * and so on...

### Test Cases

 * Program `99` will be `99` after execution, having immediately completed
 * Program `1,9,10,3,2,3,11,0,99,30,40,50` will be `1,9,10,70,2,3,11,0,99,30,40,50` after execution (position 3 is now `70`)
 * Program `1,0,0,0,99` will be `2,0,0,0,99`
 * Program `2,3,0,3,99` will be `2,3,0,6,99`
 * Program `2,4,4,5,99,0` will be `2,4,4,5,99,9801`
 * Program `1,1,1,4,99,5,6,0,99` will be `1,1,1,4,99,5,6,0,99`

### Task 

 Your puzzle input is a text file with a single line, representing a program with numbers seperated by commas. You will need to:
  * parse the file into a format you can send to your emulator
  * modify the program memory so that position `1` has a value of `12`
  * modify the program memory so that position `2` has a value of `2` ("02")
  * run the program through your emulator
  * retrieve the puzzle solution from position `0`

## Part 2 

In part 2 you will need to find which pair of 2 digit numbers in positions `1` and `2` gives the correct result in position `0`

 * There is a single number as an additional puzzle input at the bottom of the part 2 prompt.

### Test Cases

 * values `12` and `2` should give your result from part 1
 * values `0` and `0` should give a different result

### Task

Using the same program from part 1 and given all possible combonations of two 2-digit numbers, report the combo that gives the correct result in position 0.

<details>
    <summary>Hints</summary> 
* It may be possible to read the provided program and determine the solution without code

* It is likely easier to generate a list of all possible solutions and try each one at a time

* Since the emulator code works by mutating the program memory, it is important to always use a copy of the original program as input to your emulator so you don't have to read the program from the file again every time you run it
</details>

## More Info

[The Apollo Guidence Computer](https://en.wikipedia.org/wiki/Apollo_Guidance_Computer)

[Apollo 11's "1202" alarm](https://www.discovermagazine.com/the-sciences/apollo-11s-1202-alarm-explained)
