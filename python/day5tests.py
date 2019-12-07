from day5 import evaluate

def assertListEquals(a, b):
    len_a, len_b = len(a), len(b)
    assert len_a == len_b, "Expect lists to be of same length"
    for i in range(len(a)):
        assert a[i] == b[i], "Expect element at postion " + \
            str(i) + " to be " + str(b[i]) + " but found " + str(a[i])

def run_test_for_cases(cases, test):
    for input, output in cases:
        test(input, output)

OP_Add = 1
OP_Mult = 2
OP_Read = 3
OP_Write = 4
OP_JumpIfTrue = 5
OP_JumpIfFalse = 6
OP_LessThan = 7
OP_Equals = 8
OP_Stop = 99

cases = [([1, 0, 0, 0, 99], [2, 0, 0, 0, 99]),
         ([2, 3, 0, 3, 99], [2, 3, 0, 6, 99]),
         ([2, 4, 4, 5, 99, 0], [2, 4, 4, 5, 99, 9801]),
         ([1, 1, 1, 4, 99, 5, 6, 0, 99], [30, 1, 1, 4, 2, 5, 6, 0, 99]),
         ([1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50], [3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50])]
def day_2_programs_still_valid(input, output):
    evaluate(input, [])
    assertListEquals(input, output)

run_test_for_cases(cases, day_2_programs_still_valid)

def read_operation_sets_value():
    input = [OP_Read, 3, 99, 0]
    output = [OP_Read, 3, 99, 12]
    read = []
    read.append(12)
    evaluate(input, read)
    assertListEquals(input, output)

read_operation_sets_value()

def write_operation_outputs_value():
    input = [OP_Write, 2, 99]
    output = [OP_Write, 2, 99]
    read = []
    write = evaluate(input, read)
    assertListEquals(input, output)
    assert write[0] == 99

write_operation_outputs_value()

def writeOperationInImmediateModeOutputsValue():
    input = [100 + OP_Write, 5, 99]
    output = [100 + OP_Write, 5, 99]
    read = []
    write = evaluate(input, read)
    assertListEquals(input, output)
    assert write[0] == 5

writeOperationInImmediateModeOutputsValue()

def addOperationInImmediateModeCorrectlySums():
    input = [1100 + OP_Add, 5, -2, 5, 99, 0]
    output = [1100 + OP_Add, 5, -2, 5, 99, 3]
    evaluate(input, [])
    assertListEquals(input, output)

addOperationInImmediateModeCorrectlySums()

def addOperationInPartialImmediateModeCorrectlySums():
    input = [1000 + OP_Add, 4, -2, 5, 99, 0]
    output = [1000 + OP_Add, 4, -2, 5, 99, 97]
    evaluate(input, [])
    assertListEquals(input, output)

addOperationInPartialImmediateModeCorrectlySums()

cases = [(4, 0), (8, 1), (9, 0)]
def checkEqualTo8InPositionMode(input, output):
    program = [3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(cases, checkEqualTo8InPositionMode)

cases = [(4, 1), (8, 0), (9, 0)]
def checkLessThanTo8InPositionMode(input, output):
    program = [3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(cases, checkLessThanTo8InPositionMode)

cases = [(4, 0), (8, 1), (9, 0)]
def checkEqualTo8InImmediateMode(input, output):
    program = [3, 3, 1108, -1, 8, 3, 4, 3, 99 ]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(cases, checkEqualTo8InImmediateMode)

cases = [(4, 1), (8, 0), (9, 0)]
def checkLessThanTo8InImmediateMode(input, output):
    program = [3, 3, 1107, -1, 8, 3, 4, 3, 99 ]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(cases, checkLessThanTo8InImmediateMode)

cases = [(0, 0), (1, 1), (9, 1)]
def jumpTestInPositionMode(input, output):
    program = [3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 ]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(cases, jumpTestInPositionMode)

cases = [(0, 0), (1, 1), (9, 1)]
def jumpTestInImmediateMode(input, output):
    program = [3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 ]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(cases, jumpTestInImmediateMode)

cases = [(4, 999), (8, 1000), (9, 1001)]
def checkLtEqGt(input, output):
    program= [
        3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31,
        1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,
        999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99]    
    write = evaluate(program, [input])
    assert len(write) == 1
    assert write[0] == output

run_test_for_cases(cases, checkLtEqGt)
