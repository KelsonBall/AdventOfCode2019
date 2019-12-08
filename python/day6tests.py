from day6 import build_adjacency_list, get_descendants_of_node
from testutil import assert_list_equals, assert_set_equals, run_test_for_cases

test_case_1 = ["1)2", "1)3", "2)4", "3)5", "3)6", "3)7"]
test_case_2 = ["1)2", "2)3", "3)4", "4)5", "5)6", "6)7"]

all_passed = True

def test_build_adjacency_list(input, output):
    adj_list = build_adjacency_list(input)
    for key in output.keys():
        assert_list_equals(adj_list[key], output[key])

all_passed = all_passed and run_test_for_cases(
    [(test_case_1, { '1': ['2', '3'], '2': ['4'], '3': ['5', '6', '7'] }),
     (test_case_2, { '1': ['2'], '2': ['3'], '3': ['4'], '4': ['5'], '5': ['6'] })],
    test_build_adjacency_list)

def test_get_all_descendants(input, output):
    source, test_node = input
    adj_list = build_adjacency_list(source)
    descendants = [ x for x in get_descendants_of_node(adj_list, test_node)]
    assert_set_equals(descendants, output)

all_passed = all_passed and  run_test_for_cases(
    [( (test_case_1, '1'), [ '2', '3', '4', '5', '6', '7']),
     ( (test_case_1, '2'), [ '4']),
     ( (test_case_1, '3'), [ '5', '6', '7']),
     ( (test_case_2, '1'), [ str(i) for i in range(2, 8)]),
     ( (test_case_2, '2'), [ str(i) for i in range(3, 8)])],
    test_get_all_descendants)

print('Passed' if all_passed else 'Failed')