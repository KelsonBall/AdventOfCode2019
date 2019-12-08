package kelson.advent.day5

import java.lang.Exception

abstract class Instruction(program: Program)
{
    private val instruction: Int = program[0]
    private val opCode: Int = instruction % 100
    abstract fun paramCount(): Int
    private val flags: ParamFlags = ParamFlags(instruction - opCode, paramCount())
    val parameters: Array<Parameter> = (1..paramCount()).map { flags.get(it)(program[it]) }.toTypedArray()

    // Parameter accessors
    fun a() : Int = parameters[0].value
    fun a(program: Program) : Int = parameters[0].result(program)
    fun b() : Int = parameters[1].value
    fun b(program: Program) : Int = parameters[1].result(program)
    fun c() : Int = parameters[2].value
    fun c(program: Program) : Int = parameters[2].result(program)

    abstract fun evaluate(program: Program)
}

fun parseInstruction(program: Program) : Instruction =
    when (program[0] % 100) {
        1 -> AddInstruction(program)
        2 -> MultiplyInstruction(program)
        3 -> ReadInstruction(program)
        4 -> WriteInstruction(program)
        5 -> JumpIfTrueInstruction(program)
        6 -> JumpIfFalseInstruction(program)
        7 -> LessThanInstruction(program)
        8 -> EqualInstruction(program)
       99 -> StopInstruction(program)
     else -> throw Exception("Invalid op code: ${program[0]}")
    }

class AddInstruction(program: Program) : Instruction(program) {

    override fun paramCount(): Int = 3

    override fun evaluate(program: Program) {
        program.memory[c()] = a(program) + b(program)
        program.skip(this)
    }
}

class MultiplyInstruction(program: Program) : Instruction(program) {

    override fun paramCount(): Int = 3

    override fun evaluate(program: Program) {
        program.memory[c()] = a(program) * b(program)
        program.skip(this)
    }
}

class ReadInstruction(program: Program) : Instruction(program) {

    override fun paramCount(): Int = 1

    override fun evaluate(program: Program) {
        val value = program.system.read()
        if (value is Int) {
            program.memory[a()] = value
            program.skip(this)
        }
        else {
            program.state = State.Suspended
        }
    }
}

class WriteInstruction(program: Program) : Instruction(program) {

    override fun paramCount(): Int = 1

    override fun evaluate(program: Program) {
        program.system.write(a(program))
        program.skip(this)
    }
}

class JumpIfTrueInstruction(program: Program) : Instruction(program) {

    override fun paramCount(): Int = 2

    override fun evaluate(program: Program) {
        if (a(program) == 0)
            program.skip(this)
        else
            program.jumpTo(b(program))
    }
}

class JumpIfFalseInstruction(program: Program) : Instruction(program) {

    override fun paramCount(): Int = 2

    override fun evaluate(program: Program) {
        if (a(program) != 0)
            program.skip(this)
        else
            program.jumpTo(b(program))
    }
}

class LessThanInstruction(program: Program) : Instruction(program) {

    override fun paramCount(): Int = 3

    override fun evaluate(program: Program) {
        program.memory[c()] = if (a(program) < b(program)) 1 else 0
        program.skip(this)
    }
}

class EqualInstruction(program: Program) : Instruction(program) {

    override fun paramCount(): Int = 3

    override fun evaluate(program: Program) {
        program.memory[c()] = if (a(program) == b(program)) 1 else 0
        program.skip(this)
    }
}

class StopInstruction(program: Program) : Instruction(program) {

    override fun paramCount(): Int = 0

    override fun evaluate(program: Program) {
        program.state = State.Completed
    }
}
