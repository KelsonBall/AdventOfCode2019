package kelson.advent.day2

import kelson.advent.assertSequenceEquals
import kelson.advent.runAsTest
import java.io.File

const val ADD_OP = 1
const val MULT_OP = 2
const val STOP_OP = 99

fun evaluate(program: IntArray, index: Int = 0) : IntArray {
    return when (program[index]) {
        STOP_OP -> program
        ADD_OP -> add(program, index)
        MULT_OP -> mult(program, index)
        else -> throw Error()
    }
}

fun add(program : IntArray, index: Int) : IntArray {
    program[program[index + 3]] = program[program[index + 1]] + program[program[index + 2]]
    return evaluate(program, index + 4)
}

fun mult(program : IntArray, index: Int) : IntArray {
    program[program[index + 3]] = program[program[index + 1]] * program[program[index + 2]]
    return evaluate(program, index + 4)
}



fun part1Test() {
    for ((input, output) in listOf(
        Pair(intArrayOf(1, 0, 0, 0, 99), intArrayOf(2, 0, 0, 0, 99)),
        Pair(intArrayOf(2, 3, 0, 3, 99), intArrayOf(2, 3, 0, 6, 99)),
        Pair(intArrayOf(2, 4, 4, 5, 99, 0), intArrayOf(2, 4, 4, 5, 99, 9801)),
        Pair(intArrayOf(1, 1, 1, 4, 99, 5, 6, 0, 99), intArrayOf(30, 1, 1, 4, 2, 5, 6, 0, 99)),
        Pair(intArrayOf(1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50), intArrayOf(3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50)))) {
        val result = evaluate(input)
        result.assertSequenceEquals(output)
    }
}

fun runTests() {
    println("Day 2:")
    runAsTest("Part 1", ::part1Test)
}

fun run() {
    println("--- Day 2: 1202 Program Alarm ---")
    val program = File("../resources/day2/input.txt")
        .readLines()
        .single()
        .split(',')
        .map(String::toInt)
        .toIntArray()

    println("Part 1 - 12, 02 program")
    run {
        val copy = program.copyOf()
        copy[1] = 12
        copy[2] = 2
        val result = evaluate(copy)
        println(result[0])
    }

    println("Part 2 - Program that evals to 19690720")
    run {
        val inputs = sequence {
            for (noun in 0..99)
                for (verb in 0..99)
                    yield(Pair(noun, verb))
        }

        val code = inputs.map { (noun, verb) ->
            val copy = program.copyOf()
            copy[1] = noun
            copy[2] = verb
            Triple(noun, verb, evaluate(copy)[0])
        }.filter { it.third == 19690720 }
            .map { (noun, verb, _) -> "$noun$verb" }
            .first()
        println(code)
    }
}