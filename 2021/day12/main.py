import typing as T
from pprint import pprint

with open("input.txt") as f:
    data = f.read().split("\n")[:-1]
PART = 2

# Part 1
class Node:
    def __init__(self, name):
        self.name = name

    def __repr__(self) -> str:
        return self.name


class Edge:
    start: Node
    end: Node

    def __init__(self, start, end):
        self.start = start
        self.end = end

    def __repr__(self):
        return f"{self.start.name} <-> {self.end.name}"


class Path:
    nodes: T.List[Node]
    has_visited_small_cave: bool

    def __init__(self, nodes):
        self.has_visited_small_cave = False
        self.nodes = nodes

    def __repr__(self):
        return " -> ".join([n.name for n in self.nodes])


class CaveMap:
    def __init__(self, lines):
        self.lines = lines
        self.build_nodes()

    def build_nodes(self):
        self.nodes = []
        self.edges = []

        for line in self.lines:
            for part in line.split("-"):
                if not [n for n in self.nodes if n.name == part]:
                    self.nodes.append(Node(part))

        self.edges = []
        for line in self.lines:
            start, end = line.split("-")
            if not self.edge_exists(start, end):
                start_node = [n for n in self.nodes if n.name == start][0]
                end_node = [n for n in self.nodes if n.name == end][0]
                self.edges.append(Edge(start_node, end_node))

    def edge_exists(self, start_name, end_name):
        for edge in self.edges:
            if edge.start.name == start_name and edge.end.name == end_name:
                return True
        return False

    @property
    def start_node(self):
        return [n for n in self.nodes if n.name == "start"][0]

    def get_connected_nodes(self, node):
        connected_nodes = []
        for edge in self.edges:
            if edge.start == node or edge.end == node:
                other = edge.start if node == edge.end else edge.end
                if other not in connected_nodes:
                    connected_nodes.append(other)
        return connected_nodes

    def get_valid_continuations(self, path: Path) -> T.List[Path]:
        last_node = path.nodes[-1]
        connected_nodes = self.get_connected_nodes(last_node)

        continuations = []
        for node in connected_nodes:
            if node.name == "start":
                continue
            if node.name.lower() == node.name:
                if PART != 2 and node in path.nodes:
                    continue
                if PART == 2 and node in path.nodes:
                    if path.has_visited_small_cave:
                        continue

            new_path = Path([*path.nodes, node])
            new_path.has_visited_small_cave = path.has_visited_small_cave
            if node.name == node.name.lower() and node in path.nodes:
                new_path.has_visited_small_cave = True
            continuations.append(new_path)

        return continuations


cave_map = CaveMap(data)
paths = []

paths_to_explore = [
    Path([cave_map.start_node, other])
    for other in cave_map.get_connected_nodes(cave_map.start_node)
]

while len(paths_to_explore) > 0:
    new_paths_to_explore = []
    for path in paths_to_explore:
        for continuation in cave_map.get_valid_continuations(path):
            if continuation.nodes[-1].name == "end":
                paths.append(continuation)
            else:
                new_paths_to_explore.append(continuation)
    paths_to_explore = new_paths_to_explore
print(len(paths))
