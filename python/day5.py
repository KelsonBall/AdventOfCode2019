
def immediate(program_data, index):
    """Get a parameter value in immediate mode"""
    return program_data[index]

def position(program_data, index):
    """Get a parameter value in position mode"""
    return program_data[program_data[index]]

def next(pointer, params):
    return pointer + len(params) + 1

def add_operation(program_data, pointer, parameters, write, read):
    program_data[parameters[2]] = parameters[0] + parameters[1]
    return next(pointer, parameters)

def mult_operation(program_data, pointer, parameters, write, read):
    program_data[parameters[2]] = parameters[0] * parameters[1]
    return next(pointer, parameters)

def read_operation(program_data, pointer, parameters, write, read):
    program_data[parameters[0]] = read.pop(0)
    return next(pointer, parameters)

def write_operation(program_data, pointer, parameters, write, read):
    write.append(parameters[0])
    return next(pointer, parameters)

def jump_if_true_operation(program_data, pointer, parameters, write, read):
    return parameters[1] if parameters[0] != 0 else next(pointer, parameters)

def jump_if_false_operation(program_data, pointer, parameters, write, read):
    return parameters[1] if parameters[0] == 0 else next(pointer, parameters)

def less_than_operation(program_data, pointer, parameters, write, read):
    program_data[parameters[2]] = 1 if parameters[0] < parameters[1] else 0
    return next(pointer, parameters)

def equals_operation(program_data, pointer, parameters, write, read):
    program_data[parameters[2]] = 1 if parameters[0] == parameters[1] else 0
    return next(pointer, parameters)

def stop_operation(program_data, pointer, parameters, write, read):
    return -1

class Operand:
    def __init__(self, parameter_count, mode_overrides, action):        
        self.default_modes = mode_overrides
        self.parameter_count = parameter_count
        self.action = action

    def get_mode_overrides(self, parsed_modes):
        modes_copy = [ position if i >= len(parsed_modes) else parsed_modes[i] for i in range(self.parameter_count) ]
        if self.default_modes:
            for key in self.default_modes.keys():
                modes_copy[key] = self.default_modes[key]
        return modes_copy

    # modes (program_data, index) -> integer
    def evaluate(self, program_data, index, modes, write, read):
        modes = self.get_mode_overrides(modes)
        parameters = [modes[i](program_data, index + i + 1) for i in range(self.parameter_count)]
        return self.action(program_data, index, parameters, write, read)

operands = {
    1 : Operand(parameter_count=3, mode_overrides={2: immediate}, action=add_operation),
    2 : Operand(parameter_count=3, mode_overrides={2: immediate}, action=mult_operation),
    3 : Operand(parameter_count=1, mode_overrides={0: immediate}, action=read_operation),
    4 : Operand(parameter_count=1, mode_overrides=None, action=write_operation),
    5 : Operand(parameter_count=2, mode_overrides=None, action=jump_if_true_operation),
    6 : Operand(parameter_count=2, mode_overrides=None, action=jump_if_false_operation),
    7 : Operand(parameter_count=3, mode_overrides={2: immediate}, action=less_than_operation),
    8 : Operand(parameter_count=3, mode_overrides={2: immediate}, action=equals_operation),
    99: Operand(parameter_count=0, mode_overrides=None, action=stop_operation),
}

def get_modes_and_op_code(instruction):
    """
    Last two digits are OP code, all other digits indiciate mode flags
    Return (modes, OP)
    """
    as_text = str(instruction)
    code = int(as_text[-2:])
    mode_flags = as_text[:-2]
    mode_flags_in_order = mode_flags[-1::-1]
    modes = [ immediate if flag == '1' else position for flag in mode_flags_in_order ]
    return modes, code

def evaluate(program_data, read):
    """
    Evaluate an intcode program, outputing to the write list and reading from the read list
    """
    write = []
    read = read or []
    pointer = 0
    while pointer >= 0:
        modes, op = get_modes_and_op_code(program_data[pointer])
        pointer = operands[op].evaluate(program_data, pointer, modes, write, read)
    return write

if __name__ == "__main__":
    with open("../resources/day5/input.txt") as file:
        program = [ int(x) for x in file.readline().split(',') ]
    print("part 1:")
    copy = list(program)
    output = evaluate(copy, [1])
    print(output)
    assert output[-1] == 4511442

    print("part 2:")
    copy = list(program)
    output = evaluate(copy, [ 5 ])
    print(output)
    assert output[-1] == 12648139