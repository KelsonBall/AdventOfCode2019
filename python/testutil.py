
def assert_list_equals(a, b):
    len_a, len_b = len(a), len(b)
    assert len_a == len_b, "Expect lists to be of same length"
    for i in range(len(a)):
        assert a[i] == b[i], "Expect element at postion " + \
            str(i) + " to be " + str(b[i]) + " but found " + str(a[i])

def assert_set_equals(a, b):
    len_a, len_b = len(a), len(b)
    assert len_a == len_b, "Expect lists to be of same length"
    assert len(set(a) & set(b)) == len_a, "Expceted intersection to maintain keep items"

def run_test(test):
    print('running test: ' + str(test))
    try:        
        test()
        print('1/1 ✔')
        return True
    except AssertionError:
        print(' >> failed')
        print('0/1 ❌')
        return False

def run_test_for_cases(cases, test):
    print('running test: ' + str(test))
    ran, passed = 0, 0
    for input, output in cases:
        try:
            ran += 1
            test(input, output)
            passed += 1
        except AssertionError:
            print(' >> failed for ' + str(input) + ' mapping to ' + str(output))
    print(str(passed) + '/' + str(ran) + (' ✔' if ran == passed else ' ❌'))
    return ran == passed