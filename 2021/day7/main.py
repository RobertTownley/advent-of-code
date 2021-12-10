import functools

PART = 2
with open("input.txt") as f:
    locations = [int(x) for x in f.read().split(",")]
start = min(locations)
end = max(locations)


def get_fuel_consumption_part_1(pos) -> int:
    return functools.reduce(lambda acc, crab: acc + abs(crab - pos), locations, 0)


COST_MAP = {}


def get_fuel_consumption_part_2(pos):
    total = 0
    for crab in locations:
        to_move = abs(crab - pos)
        if to_move not in COST_MAP:
            COST_MAP[to_move] = functools.reduce(
                lambda acc, x: acc + x, range(1, to_move + 1), 0
            )

        total += COST_MAP[to_move]
    return total


lowest, position = None, None
for possible_position in range(start, end):
    if PART == 1:
        fuel_consumption = get_fuel_consumption_part_1(possible_position)
    else:
        fuel_consumption = get_fuel_consumption_part_2(possible_position)
    if not lowest or fuel_consumption < lowest:
        lowest = fuel_consumption
        position = possible_position
print(f"Part {PART}\nPosition: {position}\nFuel Consumption: {lowest}\n\n")
