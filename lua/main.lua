local days = {
    require "day1"
}

function flag(name)
    local flag = '-' .. name
    if #name > 1 then
        flag = '-' .. flag
    end
    for key,value in pairs(arg) do
        if value == flag then
            return true
        end
    end
    return false
end

local run_tests = flag("test") or flag("t")

for day,actions in pairs(days) do
    header = "--- Day " .. day .. ": "
    if actions.title then
        header = header .. actions.title
    end
    header = header .. " ---"
    print(header)
    if run_tests and actions.tests then 
        actions.tests()
    end
    if actions.pre then
        print("Loading...")
        actions.pre()
    end
    print("Part 1:")
    actions.part1()
    print("Part 2:")
    actions.part2()
    if actions.post then
        print("Cleaning up...")
        actions.post()
    end
end