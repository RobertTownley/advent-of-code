import pandas as pd

# filepath = "input.txt"
filepath = "sample.txt"

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
current_value = None
increase_count = 0
