

def build_adjacency_list(lines):
    nodes = {}
    for line in lines:
        elements = line.split(')')
        key = elements[0]
        value = elements[1]
        if not key in nodes.keys():
            nodes[key] = []
        if not value in nodes.keys():
            nodes[value] = []
        nodes[key].append(value)
    return nodes

def get_children_of_node(data, node):    
    for child in data[node]:
        yield child

def get_descendants_of_node(data, node):    
    for child in data[node]:
        yield child
        for descendant in get_descendants_of_node(data, child):
            yield descendant

def get_parent_of_node(data, node):
    for (parent, children) in data.items():
        if node in children:
            return parent

def get_ancestors_of_node(data, node):
    parent = get_parent_of_node(data, node)
    while node != None:
        yield parent
        parent = get_parent_of_node(data, node)

def get_depth_of_node(data, node):
    return len([x for x in get_parent_of_node(data, node)])