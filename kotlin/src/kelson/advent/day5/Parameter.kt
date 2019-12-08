package kelson.advent.day5

interface Parameter {
    val value : Int
    fun result(program: Program) : Int
}

class Position(override val value: Int) : Parameter {
    override fun result(program: Program) : Int {
        return program.memory[value]
    }
}

class Immediate(override val value: Int) : Parameter {
    override fun result(program : Program) : Int {
        return value;
    }
}