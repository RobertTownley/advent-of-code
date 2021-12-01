import pandas as pd

filepath = "input.txt"
# filepath = "sample.txt"

df = pd.read_csv(filepath, names=["value"])
current_value = None
increase_count = 0

for index, row in df.iterrows():
    value = row["value"]
    if current_value and value > current_value:
        increase_count += 1
    current_value = value

print(f"The value increased {increase_count} times")
