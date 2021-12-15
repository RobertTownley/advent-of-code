with open("sample.txt") as f:
    data = f.read().split("\n")[:-1]


class Octopus:
    energy_level: int
    has_flashed_this_step: bool
    x: int
    y: int

    def __init__(self, energy_level, x, y):
        self.has_flashed_this_step = False
        self.energy_level = energy_level
        self.x = x
        self.y = y

    def __str__(self):
        return str(self.energy_level)


class OctopusMap:
    def __init__(self, lines):
        self.lines = lines
        self.octopi = {}
        self.flashes = 0
        self.build_map()

    def __str__(self):
        rows = []
        for row in self.octopi.values():
            rows.append("".join([str(o) for o in row.values()]))
        return "\n".join(rows) + "\n"

    def build_map(self):
        for x, line in enumerate(self.lines):
            self.octopi[x] = {}
            for y, char in enumerate(line):
                self.octopi[x][y] = Octopus(int(char), x, y)

    @property
    def all_octopi(self):
        octopi = []
        for k, line in self.octopi.items():
            octopi += [o for o in line.values()]
        return octopi

    def conduct_step(self):
        for o in self.all_octopi:
            o.has_flashed_this_step = False
            o.energy_level += 1

        flash_occurred = True
        while flash_occurred:
            flash_occurred = False
            for line in self.octopi.values():
                for o in line.values():
                    if o.energy_level > 9 and not o.has_flashed_this_step:
                        o.has_flashed_this_step = True
                        o.energy_level = 0
                        flash_occurred = True
                        self.flashes += 1

                        # Flash nearby octopi
                        for other_o in self.get_adjacents(o):
                            other_o.energy_level += 1

        for o in self.all_octopi:
            if o.has_flashed_this_step:
                o.energy_level = 0

    def get_adjacents(self, o: Octopus):
        adjacents = []

        for x, y in [
            (o.x + i, o.y + j)
            for i in (-1, 0, 1)
            for j in (-1, 0, 1)
            if i != 0 or j != 0
        ]:
            if self.octopi.get(x, {}).get(y, None):
                adjacents.append(self.octopi[x][y])
        return adjacents


octomap = OctopusMap(data)
for i in range(0, 100):
    octomap.conduct_step()
    print(octomap)
    print(f"Step: {i}, Flashes: {octomap.flashes}")
