def read_lines_from_file(filename):
    with open(filename) as f:
        data = f.read().split("\n")
        if not data[-1]:
            data = data[:-1]
    return data


class Point:
    def __init__(self, value, x, y):
        self.is_low_point = False
        self.height = int(value)
        self.x = x
        self.y = y

    def __str__(self):
        return f"Point ({self.x},{self.y})"

    @property
    def risk_level(self):
        return self.height + 1


class Report:
    def __init__(self, lines):
        self.lines = lines
        self.height_map = {}
        self.max_x = 0
        self.max_y = 0
        self.total_risk = 0

        for x, line in enumerate(self.lines):
            self.height_map[x] = {}
            for y, value in enumerate(line):
                self.height_map[x][y] = Point(value, x, y)
                if y > self.max_y:
                    self.max_y = y

            if x > self.max_x:
                self.max_x = x

    def get_adjacent_points(self, x, y):
        points = []
        if x - 1 >= 0:
            points.append(self.height_map[x - 1][y])
        if y - 1 >= 0:
            points.append(self.height_map[x][y - 1])
        if x + 1 <= self.max_x:
            points.append(self.height_map[x + 1][y])
        if y + 1 <= self.max_y:
            points.append(self.height_map[x][y + 1])
        return points

    def compute_low_points(self):
        for x, rows in self.height_map.items():
            for y, point in rows.items():
                adjacent_points = self.get_adjacent_points(x, y)
                if all([p.risk_level > point.risk_level for p in adjacent_points]):
                    point.is_low_point = True

                    # Add to risk value
                    self.total_risk += point.risk_level


lines = read_lines_from_file("input.txt")
report = Report(lines)
report.compute_low_points()
print(f"The total risk level is {report.total_risk}")
