package kelson.advent

import kelson.advent.day1.testPart1ForModuleFuelRequirements
import kotlin.test.assertEquals

fun IntArray.assertSequenceEquals(other : IntArray)
{
    for (i in other.indices)
        assertEquals(this[i], other[i])
}

fun runAsTest(name : String, test : () -> Unit) {
    print("$name: ")
    try {
        testPart1ForModuleFuelRequirements()
        println("✔")
    }
    catch (_ : Throwable) {
        println("❌")
    }
}