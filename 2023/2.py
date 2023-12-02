from functools import reduce

USE_SAMPLE = False
PART = 1


sample_input = """Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"""
sample_answer = 8


with open("2.txt") as f:
    real_input = f.read()


text = sample_input if USE_SAMPLE else real_input
lines = [line for line in text.split("\n") if line]
data = {}

CRITERIA = {
    "red": 12,
    "green": 13,
    "blue": 14,
}
CUBE_COUNT = sum(CRITERIA.values())


def game_is_possible(row: str, CRITERIA):
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
    index = int(game_title.split(" ")[1])
    if game_is_possible(row, CRITERIA):
        answer += index
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


answer = 0
for line in lines:
    game_title, row = line.split(": ")
    answer += get_row_power(row)
print(f"The answer to part 2 is {answer}")
