import pandas as pd

filepath = "input.txt"
# filepath = "sample.txt"

df = pd.read_csv(filepath, names=["value"])

# Part 1: Count the # of times the items in the array increase
current_value = None
increase_count = 0
values = df["value"]
for value in values:
    if current_value and value > current_value:
        increase_count += 1
    current_value = value

print(f"In part 1, the value increased {increase_count} times")

# Part 2: Count the # of times the sliding 3 window increased
pairings = [[values[0]]]
sums = []
for value in values[1:]:
    previous_pairing = pairings[-1]
    pairings.append([*previous_pairing, value][-3:])
    if len(pairings[-1]) == 3:
        sums.append(sum(pairings[-1]))

current_value = None
increase_count = 0
for value in sums:
    if current_value and value > current_value:
        increase_count += 1
    current_value = value

print(f"In part 2, the value increased {increase_count} times")
