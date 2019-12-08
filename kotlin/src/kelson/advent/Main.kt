package kelson.advent

fun runDayScript(day : Int)
{
    when (day) {
        1 -> kelson.advent.day1.run()
        2 -> kelson.advent.day2.run()
        5 -> kelson.advent.day5.run()
    }
}

fun runTests()
{
    kelson.advent.day1.runTests()
    kelson.advent.day2.runTests()
}

fun main(args : Array<String>)
{
    if ("test" in args)
        runTests()
    else if ("day" in args || "days" in args)
        args
            .filter{ it[0].isDigit() }
            .map(String::toInt)
            .forEach{ runDayScript(it) }
}