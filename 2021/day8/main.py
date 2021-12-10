from pprint import pprint

with open("input.txt") as f:
    data = [x.split(" | ")[1] for x in filter(lambda x: bool(x), f.read().split("\n"))]

UNIQUE_VALUES = [7, 4, 3, 2]
total = 0
for output in data:
    for segment in output.split(" "):
        if len(segment) in UNIQUE_VALUES:
            total += 1

print(total)
