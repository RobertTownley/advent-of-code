import datetime
from functools import cached_property


class Rule:
    def __init__(self, text):
        chars, result = text.split(" -> ")
        self.start = chars[0]
        self.end = chars[1]
        self.result = result

    def __repr__(self):
        return f"{self.start}_{self.end} -> {self.result}"

    @cached_property
    def needle(self):
        return f"{self.start}{self.end}"


with open("input.txt") as f:
    template, rules = f.read().split("\n\n")
    rules = [Rule(text) for text in rules.split("\n")[:-1]]


"""
def conduct_step(t1, rules):
    t2 = []
    t1_len = len(t1)
    for i, char in enumerate(t1):
        if i + 1 >= t1_len:
            t2.append(char)
            break
        t2.append(char)
        for rule in rules:
            if rule.start == char and rule.end == t1[i + 1]:
                t2.append(rule.result)
                break
    return t2
"""


RULE_MAP = {}
for r in rules:
    RULE_MAP[r.needle] = r.result


def conduct_step(t1):
    t2 = ""
    t1_len = len(t1)

    for i, char in enumerate(t1):
        t2 += char
        if i + 1 >= t1_len:
            break
        next_char = t1[i + 1]
        pairing = f"{char}{next_char}"

        if result := RULE_MAP[pairing]:
            t2 += result
    return t2


start = datetime.datetime.now()
for i in range(40):
    template = conduct_step(template)
    duration = datetime.datetime.now() - start
    print(i, len(template), f"{duration.seconds}.{duration.microseconds}")

uniques = "".join(set(template))
lowest = None
highest = None
for char in uniques:
    count = template.count(char)
    if not lowest or lowest > count:
        lowest = count
    if not highest or highest < count:
        highest = count
print(highest - lowest)
