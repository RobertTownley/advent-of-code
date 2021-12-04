class Cell:
    def __init__(self, value):
        self.value = value
        self.marked = False

    def __repr__(self):
        return "X" if self.marked else self.value

class Board:

    def __init__(self, text):
        self.latest_value = 0
        self.text = text
        self.create_cells()

    def __str__(self):
        rows = [row for row in self.cells.values()]
        cell_arrays = [list(row.values()) for row in rows]
        return "\n".join([str(x) for x in cell_arrays])

    def create_cells(self):
        self.cells = {}
        for x, row in enumerate(self.text.split("\n")):
            self.cells[x] = {}
            for y, cell_value in enumerate(row.split()):
                self.cells[x][y] = Cell(cell_value)

    def mark_number(self, number):
        for row in self.cells.values():
            for cell in row.values():
                if cell.value == number:
                    self.latest_value = int(number)
                    cell.marked = True
                    # TODO: Can a number appear more than once on a board?
                    return

    @property
    def is_complete(self):
        for i in range(0, 5):
            # Horizontal
            row = self.cells[i]
            if all([cell.marked for cell in row.values()]):
                return True

            # Vertical
            column = [self.cells[x][i] for x in range(0, 5)]
            if all([cell.marked for cell in column]):
                return True

        return False

    @property
    def score(self):
        return self.sum_of_cells * self.latest_value

    @property
    def sum_of_cells(self):
        total = 0
        marked = 0
        for row in self.cells.values():
            for cell in row.values():
                if not cell.marked:
                    marked += 1
                    total += int(cell.value)
        return total


filepath = 'input.txt'
def parse_drawings_and_boards():
    with open(filepath) as f:
        data = f.read()
    split_text = data.split("\n\n")
    drawings = split_text[0].split(",")
    boards = [Board(text) for text in split_text[1:]]
    return [drawings, boards]



# Part 1
winner = None
drawings, boards = parse_drawings_and_boards()

for drawing in drawings:
    if winner:
        break
    for board in boards:
        board.mark_number(drawing)
        if board.is_complete and not winner:
            winner = board
            break

if winner:
    print(f"The winning board in part 1 will have a score of {winner.score}")


# Part 2
loser = None
drawings, boards = parse_drawings_and_boards()
incomplete_boards = boards
for drawing in drawings:
    if len(incomplete_boards) == 0:
        break
    for board in boards:
        board.mark_number(drawing)

    incomplete_boards = list(filter(lambda b: b.is_complete == False, boards))
    if len(incomplete_boards) == 1:
        loser = incomplete_boards[0]

if loser:
    print(loser.latest_value)
    print(f"The losing board in part 2 will have a score of {loser.score}")
