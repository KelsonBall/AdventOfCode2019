require "util"
require "test"

local Process = {}

Process.__index = Process

setmetatable(Process, {
    __call = function(prototype, source, read, write)
        if type(source) == "function" then
            source = source()
        end
        local process = setmetatable({
            program = source,
            read = write,
            input_queue = {},
            write = read,
            output_queue = {},
            pc = 1,
            state = Process.CONTINUE
        }, Process)
        return process
    end
})

function Process:CONTINUE(params)
    self.pc = self.pc + params + 2
    return Process.CONTINUE
end

function Process:STOP(params) return Process.STOP end

function Process:get_ptr(index)
    local value = self.program[self.pc + index] + 1
    local result = self.program[value]
    return result
end

function Process:set_ptr(index, value)
    self.program[self.program[self.pc + index] + 1] = value
end

function Process:current()
    return self.program[self.pc]
end

function Process:add()    
    self:set_ptr(3, self:get_ptr(1) + self:get_ptr(2))
    return self:CONTINUE(2)
end

function Process:mult()    
    self:set_ptr(3, self:get_ptr(1) * self:get_ptr(2))
    return self:CONTINUE(2)
end

function Process:stop()
    return self:STOP(0)
end

Process.ops = { [1] = Process.add, [2] = Process.mult, [99] = Process.stop }

function Process:evaluate()
    while self.state == Process.CONTINUE do
        local operation = self.ops[self:current()]
        self.state = operation(self)
    end
    return self.state
end

local function test()
    doTests{
        ["Operation Evaluation"] = {
            { { { 1, 0, 0, 0, 99 }, { 2, 0, 0, 0, 99 } },
              { { 2, 3, 0, 3, 99 }, { 2, 3, 0, 6, 99 } },
              { { 2, 4, 4, 5, 99, 0 }, { 2, 4, 4, 5, 99, 9801 } },
              { { 1, 1, 1, 4, 99, 5, 6, 0, 99 }, { 30, 1, 1, 4, 2, 5, 6, 0, 99 } },
              { { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }, { 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 } },
            },
            function (_, program, expected)
                local process = Process(program)
                local status = process:evaluate()
                assert(status == Process.STOP)
                assertTableEquals(process.program, expected)
            end
        }
    }
end

local function runDay1()

end

local function runDay2()

end

return {
    ["title"] = "1202 Program Alarm",
    ["tests"] = test,    
    ["part1"] = runDay1,
    ["part2"] = runDay2,
}