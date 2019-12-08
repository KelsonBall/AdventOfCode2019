from day5 import evaluate
from testutil import assert_list_equals, run_test, run_test_for_cases

OP_Add = 1
OP_Mult = 2
OP_Read = 3
OP_Write = 4
OP_JumpIfTrue = 5
OP_JumpIfFalse = 6
OP_LessThan = 7
OP_Equals = 8
OP_Stop = 99

def day_2_programs_still_valid(input, output):
    evaluate(input, [])
    assert_list_equals(input, output)

run_test_for_cases(
        [([1, 0, 0, 0, 99], [2, 0, 0, 0, 99]),
         ([2, 3, 0, 3, 99], [2, 3, 0, 6, 99]),
         ([2, 4, 4, 5, 99, 0], [2, 4, 4, 5, 99, 9801]),
         ([1, 1, 1, 4, 99, 5, 6, 0, 99], [30, 1, 1, 4, 2, 5, 6, 0, 99]),
         ([1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50], [3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50])], 
         day_2_programs_still_valid)

def read_operation_sets_value():
    input = [OP_Read, 3, 99, 0]
    output = [OP_Read, 3, 99, 12]
    read = []
    read.append(12)
    evaluate(input, read)
    assert_list_equals(input, output)

run_test(read_operation_sets_value)

def write_operation_outputs_value():
    input = [OP_Write, 2, 99]
    output = [OP_Write, 2, 99]
    read = []
    write = evaluate(input, read)
    assert_list_equals(input, output)
    assert write[0] == 99

run_test(write_operation_outputs_value)

def write_operation_in_immediate_mode_outputs_value():
    input = [100 + OP_Write, 5, 99]
    output = [100 + OP_Write, 5, 99]
    read = []
    write = evaluate(input, read)
    assert_list_equals(input, output)
    assert write[0] == 5

run_test(write_operation_in_immediate_mode_outputs_value)

def add_operation_in_immediate_mode_correctly_sums():
    input = [1100 + OP_Add, 5, -2, 5, 99, 0]
    output = [1100 + OP_Add, 5, -2, 5, 99, 3]
    evaluate(input, [])
    assert_list_equals(input, output)

run_test(add_operation_in_immediate_mode_correctly_sums)

def add_operation_in_partial_immediate_mode_correctly_sums():
    input = [1000 + OP_Add, 4, -2, 5, 99, 0]
    output = [1000 + OP_Add, 4, -2, 5, 99, 97]
    evaluate(input, [])
    assert_list_equals(input, output)

run_test(add_operation_in_partial_immediate_mode_correctly_sums)

def check_equal_to_8_in_position_mode(input, output):
    program = [3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(
    [(4, 0), (8, 1), (9, 0)], 
    check_equal_to_8_in_position_mode)

def check_less_than_8_in_position_mode(input, output):
    program = [3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(
    [(4, 1), (8, 0), (9, 0)], 
    check_less_than_8_in_position_mode)

def check_equal_to_8_in_immediate_mode(input, output):
    program = [3, 3, 1108, -1, 8, 3, 4, 3, 99 ]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(
    [(4, 0), (8, 1), (9, 0)], 
    check_equal_to_8_in_immediate_mode)

def check_less_than_8_in_immediate_mode(input, output):
    program = [3, 3, 1107, -1, 8, 3, 4, 3, 99 ]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(
    [(4, 1), (8, 0), (9, 0)], 
    check_less_than_8_in_immediate_mode)

def jump_test_in_position_mode(input, output):
    program = [3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 ]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(
    [(0, 0), (1, 1), (9, 1)], 
    jump_test_in_position_mode)

def jump_test_in_immediate_mode(input, output):
    program = [3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 ]    
    write = evaluate(program, [ input ])
    assert write[0] == output

run_test_for_cases(
    [(0, 0), (1, 1), (9, 1)], 
    jump_test_in_immediate_mode)

def check_lt_eq_gt(input, output):
    program= [
        3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31,
        1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,
        999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99]    
    write = evaluate(program, [input])
    assert len(write) == 1
    assert write[0] == output

run_test_for_cases(
    [(4, 999), (8, 1000), (9, 1001)], 
    check_lt_eq_gt)