package kelson.advent.day1

import kelson.advent.runAsTest
import java.io.File
import kotlin.test.assertEquals

fun fuelRequired(mass : Int) : Int = mass / 3 - 2

fun moduleFuelRequirement(modules : Iterable<Int>) = modules.map{ fuelRequired(it) }.sum()

fun totalFuelRequired(mass : Int) : Int {
    val fuelRequired = fuelRequired(mass)
    return if (fuelRequired <= 0)
        0
    else
        fuelRequired + totalFuelRequired(fuelRequired)
}

fun totalModuleFuelRequirement(modules : Iterable<Int>) = modules.map{ totalFuelRequired(it) }.sum()

fun testPart1ForModuleFuelRequirements() =
    listOf(
        Pair(12, 2),
        Pair(14, 2),
        Pair(1969, 654),
        Pair(100756, 33583))
    .map{ (mass, fuel) -> Pair(fuelRequired(mass), fuel) }
    .forEach { (required, expected) -> assertEquals(required, expected) }

fun testPart2ForRecursiveFuelRequirements() =
    listOf(
        Pair(14, 2),
        Pair(1969, 966),
        Pair(100756, 50346))
    .map{ (mass, fuel) -> Pair(totalFuelRequired(mass), fuel) }
    .forEach { (required, expected) -> assertEquals(required, expected) }


fun runTests() {
    println("Day 1:")
    runAsTest("Part 1", ::testPart1ForModuleFuelRequirements)
    runAsTest("Part 2", ::testPart2ForRecursiveFuelRequirements)
}

fun run() {
    println("--- DDay 1: The Tyranny of the Rocket Equation ---")
    val modules = File("../resources/day1/input.txt").readLines().map(String::toInt)

    println("Part 1 - Required fuel for modules:")
    val total = moduleFuelRequirement(modules)
    println(total)

    println("Part 2 - Required fuel for modules and fuel:")
    val biggerTotal = totalModuleFuelRequirement(modules)
    println(biggerTotal)
}