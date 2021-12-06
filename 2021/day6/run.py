FISH_MAP = {}


def add_value_to_fish_map(value):
    if value not in FISH_MAP:
        FISH_MAP[value] = 0
    FISH_MAP[value] += 1


def increment_fish_map():
    new_fish_map = {6: 0, 8: 0}
    for key, value in FISH_MAP.items():
        new_key = key - 1
        if new_key < 0:
            new_fish_map[6] += value
            new_fish_map[8] += value
        else:
            new_fish_map[new_key] = value

    return new_fish_map


with open("input.txt") as f:
    for value in f.read().split(","):
        add_value_to_fish_map(int(value))

days = 256
for i in range(0, days):
    FISH_MAP = increment_fish_map()
print(sum(FISH_MAP.values()))
