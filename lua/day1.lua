require "test"
require "util"

local function sum(table, scoreOf)
    local sum = 0
    for key, value in pairs(table) do
        sum = sum + scoreOf(value)
    end
    return sum
end

local function fuelRequired(mass)
    return math.floor(mass / 3) - 2
end

local function moduleFuelRequirement(modules)
    return sum(modules, fuelRequired)
end

local function totalFuelRequired(mass)
    local required = fuelRequired(mass)
    if required <= 0 then
        return 0
    else
        return required + totalFuelRequired(required)
    end
end

local function totalModuleFuelRequirement(modules)
    return sum(modules, totalFuelRequired)
end

local function test()
    doTests{
        ["Base fuel requirement"] = {
            { { 12, 2 }, { 14, 2 }, {1969, 654 }, { 100756, 33583 } },
            function (_, mass, fuel) 
                assert(fuelRequired(mass) == fuel)                
            end
        },
        ["Total fuel requirement"] = {
            { { 14, 2 }, { 1969, 966 }, { 100756, 50346 } },
            function(_, mass, fuel) 
                assert(totalFuelRequired(mass) == fuel)
            end
        }
    }
end

local modules = {}

local function load()
    for line in readLines("../resources/day1/input.txt") do
        table.insert(modules, line)
    end
end

local function runDay1()    
    print(moduleFuelRequirement(modules))
end

local function runDay2()
    print(totalModuleFuelRequirement(modules))
end

return {
    ["title"] = "The Tyranny of the Rocket Equation",
    ["tests"] = test,
    ["pre"] = load,
    ["part1"] = runDay1,
    ["part2"] = runDay2,
}