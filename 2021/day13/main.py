with open("input.txt") as f:
    dots, instructions_text = f.read().split("\n\n")


class Dot:
    def __init__(self, x, y):
        self.x = int(x)
        self.y = int(y)

    def __repr__(self):
        return f"#"


class Instruction:
    def __init__(self, text):
        details = text.split(" ")[2]
        direction, amount = details.split("=")
        self.direction = direction
        self.amount = int(amount)

    def __str__(self):
        return f"{self.direction}={self.amount}"


class Paper:
    def __init__(self, dots):
        self.dots = [Dot(x, y) for x, y in [l.split(",") for l in dots.split("\n")]]
        self.dot_map = {}
        self.max_x = 0
        self.max_y = 0

        for dot in self.dots:
            if not self.dot_map.get(dot.x):
                self.dot_map[dot.x] = {}
            if dot.x > self.max_x:
                self.max_x = dot.x
            if dot.y > self.max_y:
                self.max_y = dot.y
            self.dot_map[dot.x][dot.y] = dot

        self.fill_blank()

    def fill_blank(self):
        for x in range(0, self.max_x + 1):
            if not self.dot_map.get(x):
                self.dot_map[x] = {}
            for y in range(0, self.max_y + 1):
                if not self.dot_map[x].get(y):
                    self.dot_map[x][y] = "."

    def follow_instruction(self, instruction: Instruction):
        if instruction.direction == "y":
            dots_to_move = filter(lambda d: d.y > instruction.amount, self.dots)
            for dot in dots_to_move:
                self.dot_map[dot.x][dot.y] = "."
                dot.y = dot.y - (2 * (dot.y - instruction.amount))
                self.dot_map[dot.x][dot.y] = dot

        if instruction.direction == "x":
            dots_to_move = filter(lambda d: d.x > instruction.amount, self.dots)
            for dot in dots_to_move:
                self.dot_map[dot.x][dot.y] = "."
                dot.x = dot.x - (2 * (dot.x - instruction.amount))
                self.dot_map[dot.x][dot.y] = dot

    @property
    def dots_visible(self):
        count = 0
        for x in range(0, self.max_x):
            for y in range(self.max_y):
                if self.dot_map[x][y] != ".":
                    count += 1
        return count

    def __str__(self):
        value = ""
        for y in range(self.max_y + 1):
            value += "\n"
            for x in range(self.max_x + 1):
                representation = str(self.dot_map[x][y])
                value += representation
        return value + "\n"


instructions = [Instruction(text) for text in instructions_text.split("\n")[:-1]]

# Part 1
paper = Paper(dots)
paper.follow_instruction(instructions[0])
print(str(paper).count("#"))
print(f"There are {str(paper).count('#')} dots visible")

# Part 2
