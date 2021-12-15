import math
from pprint import pprint

POINTS_MAP = {
    ")": 3,
    "]": 57,
    "}": 1197,
    ">": 25137,
}

CLOSE_MAP = {
    ")": "(",
    "]": "[",
    "}": "{",
    ">": "<",
}
OPEN_MAP = {v: k for k, v in CLOSE_MAP.items()}
opens = OPEN_MAP.keys()
closes = CLOSE_MAP.keys()


def get_score_for_line(line):

    stack = []
    for char in line:
        if char in opens:
            stack.append(char)
        elif char in closes:
            if CLOSE_MAP[char] == stack[-1]:
                stack.pop()
            else:
                return POINTS_MAP[char]
    return 0


score = 0

# Part 1
with open("input.txt") as f:
    lines = f.read().split("\n")[:-1]

for line in lines:
    score += get_score_for_line(line)


print(f"Part 1 Score: {score}")

# Part 2


AUTOCOMPLETE_POINTS_MAP = {
    ")": 1,
    "]": 2,
    "}": 3,
    ">": 4,
}


def complete_line_and_get_score(line):
    score = 0
    stack = []
    for char in line:
        if char in opens:
            stack.append(char)
        elif char in closes:
            if CLOSE_MAP[char] == stack[-1]:
                stack.pop()
            else:
                return None

    for remaining in reversed(stack):
        score *= 5
        score += AUTOCOMPLETE_POINTS_MAP[OPEN_MAP[remaining]]
    return score


incomplete_line_scores = []
for line in lines:
    score = complete_line_and_get_score(line)
    if score:
        incomplete_line_scores.append(score)
incomplete_line_scores = sorted(incomplete_line_scores)

middle_index = math.floor((len(incomplete_line_scores)) / 2)
print(f"Part 2 score: {incomplete_line_scores[middle_index]}")
