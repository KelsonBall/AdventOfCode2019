local function getInvocationFunction(method, index, data)
    if type(data) == type({}) then
        return function() method(index, table.unpack(data)) end
    else
        return function() method(index, data) end
    end    
end

local function runBasicTest(test)
    if pcall(test) then
        return 1, 1
    else
        return 1, 0
    end
end

local function runComplexTest(test)
    local results = {}
    local ran, pass = 0, 0
    local cases, method = table.unpack(test)

    for index,data in ipairs(cases) do
        ran = ran + 1
        local result, error = pcall(getInvocationFunction(method, index, data))
        if result then
            pass = pass + 1
            table.insert(results, "  case " .. index .. ": ✔")
        else
            table.insert(results, "  case " .. index .. ": ❌ (" .. error .. ")")
        end
    end

    return ran, pass, results
end

local function runTest(test)
    if type(test) == type({}) then
        return runComplexTest(test)
    else
        return runBasicTest(test)
    end
end

function doTests(test_data)
    local ran, pass = 0, 0
    for name,test in pairs(test_data) do                        
        print("TEST: " .. name)
        local dran, dpass, results = runTest(test)
        ran, pass = ran + dran, pass + dpass
        
        if results then
            for k,v in ipairs(results) do 
                print(v)
            end
        end
    end
    if ran == pass then
        print("Passed " .. pass .. "/" .. ran .. " ✔")
    else
        print("Passed " .. pass .. "/" .. ran .. " ❌")
    end
end

function assertTableEquals(a, b)
    for k,v in pairs(a) do
        if a[k] ~= b[k] then
            return false
        end
    end
    return true
end