require "io"

function lines_iter(filename)
    return function()
        return readLines(filename)
    end
end

function readLines(filename)
    local file = io.open(filename, "r")
    if file == nil then
        error("No such file: ".. filename)
    else
        local is_closed = false
        return function()
            if is_closed then return nil end
            line = file:read("*l")
            if line == nil then
                is_closed = true
                file:close()
            end                
            return line
        end
    end
end

function flatCopy(table)
    local result = {}
    for k,v in pairs(table) do
        result[k] = v
    end
    return result
end