package kelson.advent.day5

import java.io.File

fun run() {
    println("--- Day 5: Sunny with a Chance of Asteroids ---")
    val memory = File("../resources/day5/input.txt")
        .readLines()
        .single()
        .split(',')
        .map(String::toInt)
        .toIntArray()

    println("Part 1: AC Diagnostics")
    run {
        val (system, device) = getDuplexSystem()
        val program = Program(memory.copyOf(), 0, State.Running, system)
        device.write(1)
        program.run()
        println(device.buffer().joinToString(", "))
    }

    println("Part 2: Thermal radiator")
    run {
        val (system, device) = getDuplexSystem()
        val program = Program(memory.copyOf(), 0, State.Running, system)
        device.write(5)
        program.run()
        println(device.read())
    }
}