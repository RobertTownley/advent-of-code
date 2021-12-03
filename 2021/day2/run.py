import pandas as pd


# Part 1
class Position:
    def __init__(self):
        self.aim = 0
        self.x = 0
        self.y = 0

    def follow_instruction(self, instruction):
        if instruction.direction == "forward":
            self.x += instruction.amount
        elif instruction.direction == "up":
            self.y -= instruction.amount
        elif instruction.direction == "down":
            self.y += instruction.amount


class Instruction:
    def __init__(self, row):
        self.direction = row.direction
        self.amount = row.amount


# Part 2
class PositionWithAim(Position):
    def follow_instruction(self, instruction):
        if instruction.direction == "forward":
            self.x += instruction.amount
            self.y += instruction.amount * self.aim
        elif instruction.direction == "up":
            self.aim -= instruction.amount
        elif instruction.direction == "down":
            self.aim += instruction.amount


# Run stuff
active_part = 2  # Are we in part 1 or part 2
df = pd.read_csv("input.txt", names=["direction", "amount"], sep=" ")
if active_part == 1:
    position = Position()
else:
    position = PositionWithAim()

for i, row in df.iterrows():
    instruction = Instruction(row)
    position.follow_instruction(instruction)

print(position.x * position.y)
