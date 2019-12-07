
class Operand:

    def __init__(self, identifier, parameter_count, default_modes):
        self.identifier = identifier
        self.default_modes = default_modes    

    def getModes(parsed_modes):
        modes_copy = list(parsed_modes) # avoid mutation of passed argument
        if self.default_modes:
            for key in self.default_modes.keys():
                modes_copy[key] = self.default_modes[key]
        return modes_copy

    def evaluate(self, program_data, index, write, read):
        raise Exception("Not implemented")

OP_Add = 1,
OP_Mult = 2,
OP_Read = 3,
OP_Write = 4,
OP_JumpIfTrue = 5,
OP_JumpIfFalse = 6,
OP_LessThan = 7,
OP_Equals = 8
OP_Stop = 99,

def getParameterCount(opcode):
    if opcode == OP_Stop:
        return 0
    if opcode == OP_Add:
        return 3
    if opcode == OP_Mult:
        return 3
    if opcode == OP_Read:
        return 1
    if opcode == OP_Write:
        return 1
    if opcode == OP_JumpIfTrue:
        return 2
    if opcode == OP_JumpIfFalse:
        return 2
    if opcode == OP_LessThan:
        return 3
    if opcode == OP_Equals:
        return 3

IS_IMMEDIATE = True
IS_POSITION = False


def getOpCodeAndModes(instruction):
    """
    Last two digits are OP code, all other digits indiciate mode flags
    Return (modes, OP)
    """
    as_text = str(instruction)
    code = int(as_text[-2:])
    modes = [flag == '1' for flag in as_text[:-2]]
    return modes, code




def getImmediateValue(program_data, index):
    """Get a parameter value in immediate mode"""
    return program_data[index]


def getPositionModeValue(program_data, index):
    """Get a parameter value in position mode"""
    return program_data[program_data[index]]


def getOperationFor(instruction):
    modes, op = getOpCodeAndModes(instruction)
    getters = [
        getImmediateValue if mode else getPositionModeValue for mode in modes]

    def addOperation(program_data, instruction_pointer, parameters, write, read):


def evaluate(program_data, write, read):
    """
    Evaluate an intcode program, outputing to the write list and reading from the read list
    """
    raise Exception("Not implemented")
