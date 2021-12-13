# NOTE: Part 2 doesn't work yet :sadsql:

import functools


def read_lines_from_file(filename):
    with open(filename) as f:
        data = f.read().split("\n")
        if not data[-1]:
            data = data[:-1]
    return data


def get_adjacent_points(height_map, x, y, max_x, max_y):
    points = []
    if x - 1 >= 0:
        points.append(height_map[x - 1][y])
    if y - 1 >= 0:
        points.append(height_map[x][y - 1])
    if x + 1 <= max_x:
        points.append(height_map[x + 1][y])
    if y + 1 <= max_y:
        points.append(height_map[x][y + 1])
    return points


class Point:
    def __init__(self, value, x, y):
        self.is_low_point = False
        self.is_in_basin = False
        self.height = int(value)
        self.x = x
        self.y = y

    def __str__(self):
        return f"Point ({self.x},{self.y})"

    @property
    def risk_level(self):
        return self.height + 1


class Basin:
    def __init__(self, point, report):
        self.center_point = point
        self.points = [point]
        self.height_map = report.height_map
        self.compute_basin_points()

    def compute_basin_points(self):
        added_new_point = True
        while added_new_point:
            added_new_point = False
            new_points = [*self.points]
            for point in self.points:
                adjacent_points = get_adjacent_points(
                    self.height_map,
                    point.x,
                    point.y,
                    report.max_x,
                    report.max_y,
                )
                for possible_point in adjacent_points:
                    if possible_point not in new_points and self.should_be_in_basin(
                        possible_point,
                        new_points,
                    ):
                        new_points.append(possible_point)
                        added_new_point = True
            self.points = new_points

    def should_be_in_basin(self, point, new_points):
        if point.height == 9:
            return False
        if point in new_points:
            return False

        adjacents = get_adjacent_points(
            self.height_map,
            point.x,
            point.y,
            report.max_x,
            report.max_y,
        )
        unclaimed_adjacents = [p for p in adjacents if p not in new_points]
        return all([p.risk_level > point.risk_level for p in unclaimed_adjacents])

    @property
    def size(self):
        return len(self.points)


class Report:
    def __init__(self, lines):
        self.lines = lines
        self.height_map = {}
        self.max_x = 0
        self.max_y = 0
        self.total_risk = 0

        # Part 2
        self.low_points = []
        self.basins = []

        for x, line in enumerate(self.lines):
            self.height_map[x] = {}
            for y, value in enumerate(line):
                self.height_map[x][y] = Point(value, x, y)
                if y > self.max_y:
                    self.max_y = y

            if x > self.max_x:
                self.max_x = x

    def compute_low_points(self):
        for x, rows in self.height_map.items():
            for y, point in rows.items():
                # Determine if point is a low point
                adjacent_points = get_adjacent_points(
                    self.height_map, x, y, self.max_x, self.max_y
                )
                if all([p.risk_level > point.risk_level for p in adjacent_points]):
                    point.is_low_point = True
                    # Add to risk value
                    self.total_risk += point.risk_level
                    self.low_points.append(point)

    def compute_basins(self):
        for point in self.low_points:
            basin = Basin(point, self)
            basin.compute_basin_points()
            self.basins.append(basin)


lines = read_lines_from_file("input.txt")
report = Report(lines)
report.compute_low_points()
print(f"The total risk level is {report.total_risk}")

# Part 2
report.compute_basins()

sorted_basins = list(
    map(lambda x: x.size, sorted(report.basins, key=lambda r: r.size, reverse=True))
)
product_of_biggest = functools.reduce(lambda acc, x: acc * x, sorted_basins[:3])
print(f"The product of the sizes of the largest 3 basins is {product_of_biggest}")
