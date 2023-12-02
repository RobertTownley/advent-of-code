from functools import reduce

with open("2.txt") as f:
    text = f.read()

lines = [line for line in text.split("\n") if line]

CRITERIA = {
    "red": 12,
    "green": 13,
    "blue": 14,
}
CUBE_COUNT = sum(CRITERIA.values())


def game_is_possible(row: str):
    for cube_set in row.split("; "):
        data = {}
        for batch in cube_set.split(", "):
            count, color = batch.split(" ")
            if color not in data:
                data[color] = 0
            data[color] += int(count)

        if sum(data.values()) > CUBE_COUNT:
            return False

        for color, count in data.items():
            if count > CRITERIA[color]:
                return False
    return True


answer = 0
for line in lines:
    game_title, row = line.split(": ")
    if game_is_possible(row):
        answer += int(game_title.split(" ")[1])
print(f"The answer to part 1 is {answer}")


def get_row_power(row: str) -> int:
    minimum = {
        "red": 0,
        "green": 0,
        "blue": 0,
    }
    for cube_set in row.split("; "):
        for batch in cube_set.split(", "):
            count, color = batch.split(" ")
            if int(count) > minimum[color]:
                minimum[color] = int(count)
    return reduce(lambda acc, item: acc * item, minimum.values())


rows = map(lambda line: line.split(": ")[1], lines)
answer = reduce(lambda acc, row: acc + get_row_power(row), rows, 0)
print(f"The answer to part 2 is {answer}")
