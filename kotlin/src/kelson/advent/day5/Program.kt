package kelson.advent.day5

class Program(val memory: IntArray, var pointer: Int, var state: State, val system: Sys) {

    operator fun get(offset: Int) : Int {
        return memory[pointer + offset]
    }

    fun skip(intstruction: Instruction) {
        pointer += intstruction.paramCount() + 1
    }

    fun jumpTo(index: Int) {
        pointer = index
    }

    fun run() {
        while (state == State.Running) {
            val instruction = parseInstruction(this)
            instruction.evaluate(this)
        }
    }
}