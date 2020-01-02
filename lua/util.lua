require "io"

return {
    readLines = function(filename)
        file = io.open(filename, "r")
        if file == nil then
            error("No such file: ".. filename)
        else
            return function()
                line = file:read("*l")
                if line == nil then
                    file:close()
                end                
                return line
            end
        end
    end,

    mapToTable = function(iter, transform)        
        local result = {}
        for value in iter do
            result[#result + 1] = transform(value)
        end
        return result
    end,

    doTests = function(test_data)
        local ran, pass = 0, 0
        for name,test in pairs(test_data) do                        
            print("TEST: " .. name)
            local results = {}
            if type(test) == type({}) then
                local cases, method = table.unpack(test)                

                for index,data in ipairs(cases) do
                    ran = ran + 1
                    local action = (function() method(index, data) end)
                    if type(data) == type({}) then
                        action = (function() method(index, table.unpack(data)) end)
                    end
                    if pcall(action) then
                        pass = pass + 1
                        table.insert(results, "  case " .. index .. ": ✔")
                    else
                        table.insert(results, "  case " .. index .. ": ❌")
                    end
                end
            elseif type(test) == type(type) then
                ran = ran + 1
                if pcall(test) then
                    pass = pass + 1                
                end
            end
            
            if results[1] ~= nil then
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
}