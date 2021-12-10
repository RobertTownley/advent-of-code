import functools

with open("input.txt") as f:
    locations = [int(x) for x in f.read().split(",")]
start = min(locations)
end = max(locations)


def get_fuel_consumption(position) -> int:
    return functools.reduce(lambda acc, crab: acc + abs(crab - position), locations, 0)


lowest, position = None, None
for possible_position in range(start, end):
    fuel_consumption = get_fuel_consumption(possible_position)
    if not lowest or fuel_consumption < lowest:
        lowest = fuel_consumption
        position = possible_position
print(f"Part 1\nPosition: {position}\nFuel Consumption: {lowest}\n\n")
