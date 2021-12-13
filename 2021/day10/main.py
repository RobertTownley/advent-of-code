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

with open("input.txt") as f:
    lines = f.read().split("\n")[:-1]

for line in lines:
    score += get_score_for_line(line)


print(score)
