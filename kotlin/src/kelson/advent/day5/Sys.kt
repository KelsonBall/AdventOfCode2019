package kelson.advent.day5

import java.util.*

interface Sys {
    fun read() : Int?
    fun write(value: Int)
}

class QueueSys: Sys {
    private val inputs : Queue<Int> = LinkedList<Int>()
    private val outputs: Queue<Int> = LinkedList<Int>()

    override fun read(): Int? = if (inputs.isNotEmpty()) inputs.poll() else null
    override fun write(value: Int) { outputs.add(value) }

    class Device(private val sys : QueueSys) : Sys {
        override fun read(): Int? = if (sys.outputs.isNotEmpty()) sys.outputs.poll() else null
        override fun write(value: Int) { sys.inputs.add(value) }

        fun buffer() : IntArray = sys.outputs.toIntArray()
    }
}

fun getDuplexSystem() : Pair<QueueSys, QueueSys.Device> {
    val system = QueueSys()
    val device = QueueSys.Device(system)
    return Pair(system, device)
}
