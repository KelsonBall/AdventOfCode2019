package kelson.advent.day5

import kotlin.math.pow
import kotlin.math.roundToInt

class ParamFlags(private val flags: Int, val count: Int) {
    fun get(index: Int) : (Int) -> Parameter {
        val flagAtIndex = (flags / (10.0.pow(index + 1)).roundToInt()) and 1
        return if (flagAtIndex == 0)
            { i -> Position(i) }
        else
            { i -> Immediate(i) }
    }
}